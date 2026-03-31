using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.HoSoNhanSu;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentsController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var identityUser = HttpContext.GetIdentityUser();
        var roles = identityUser.Role ?? "";
        string? companyCode = roles.Contains("SuperAdmin") ? null : identityUser.CompanyCode;

        var items = await _service.GetAllAsync(companyCode);
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _service.GetByIdAsync(id);
        if (item == null) return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DepartmentRequest request)
    {
        var identityUser = HttpContext.GetIdentityUser();
        var roles = identityUser.Role ?? "";
        
        if (!roles.Contains("SuperAdmin") || string.IsNullOrEmpty(request.CompanyCode))
        {
            request.CompanyCode = identityUser.CompanyCode;
        }

        var item = await _service.CreateAsync(request);
        return Ok(item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] DepartmentRequest request)
    {
        await _service.UpdateAsync(id, request);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}
