using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompaniesController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var identityUser = HttpContext.GetIdentityUser();
        var roles = identityUser.Role ?? "";
        
        var companies = await _companyService.GetAllAsync();
        
        if (!roles.Contains("SuperAdmin") && !string.IsNullOrEmpty(identityUser.CompanyCode))
        {
            companies = companies.Where(c => c.Code == identityUser.CompanyCode).ToList();
        }
        
        return Ok(companies);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetByCode(string code)
    {
        var company = await _companyService.GetByCodeAsync(code);
        if (company == null) return NotFound();
        return Ok(company);
    }
}
