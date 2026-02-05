using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Helpers;
using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AciPlatform.Api.Controllers;

[Authorize(Roles = "SuperAdmin,ADMINCOMPANY")]
[Route("api/[controller]")]
[ApiController]
public class UserRolesController : ControllerBase
{
    private readonly IUserRoleService _userRoleService;

    public UserRolesController(IUserRoleService userRoleService)
    {
        _userRoleService = userRoleService;
    }

    [HttpGet]
    public async Task<IActionResult> GePaging([FromQuery] PagingRequestModel param)
    {
        var identityUser = HttpContext.GetIdentityUser();
        int userId = identityUser.Id;
        string roles = identityUser.Role ?? "[]";

        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        var data = await _userRoleService.GetAll(userId, listRole);
        return Ok(new BaseResponseModel
        {
            TotalItems = data.Count(),
            Data = data.Skip(param.PageSize * (param.Page - 1))
            .Take(param.PageSize),
            PageSize = param.PageSize,
            CurrentPage = param.Page
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetAll([FromQuery] string? companyCode = null)
    {
        var identityUser = HttpContext.GetIdentityUser();
        var roles = identityUser.Role ?? "";
        
        string finalCode = identityUser.CompanyCode;
        
        // If SuperAdmin, allow overriding companyCode
        if (roles.Contains("SuperAdmin"))
        {
            if (!string.IsNullOrEmpty(companyCode))
                finalCode = companyCode;
            else
                finalCode = null; // Show all if no filter
        }
        else
        {
            // Regular admin sees only their company
            finalCode = identityUser.CompanyCode;
        }
        
        var userRoles = await _userRoleService.GetAll(finalCode);

        return Ok(new BaseResponseModel
        {
            Data = userRoles,
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var data = await _userRoleService.GetById(id);
        return Ok(new ObjectReturn
        {
            data = data,
            status = 200,
        });
    }

    [HttpPost]
    [HttpPut("{id}")]
    public async Task<IActionResult> Save([FromBody] UserRole userRole)
    {
        if (userRole == null)
        {
            return BadRequest(new { msg = ResultErrorConstants.MODEL_NULL });
        }
        if (String.IsNullOrEmpty(userRole.Title))
        {
            return BadRequest(new { msg = ResultErrorConstants.MODEL_MISS });
        }
        var identityUser = HttpContext.GetIdentityUser();
        var roles = identityUser.Role ?? "";

        int userId = identityUser.Id;
        
        if (userRole.Id > 0)
        {
            var existing = await _userRoleService.GetById(userRole.Id);
            if (existing != null) 
            {
                // Preserve existing critical fields if not sent or needed
                existing.Title = userRole.Title;
                existing.Note = userRole.Note;
                existing.ParentId = userRole.ParentId; // Allow updating parent
                
                // Only SuperAdmin can change CompanyCode, or leave as is
                if (roles.Contains("SuperAdmin") && !string.IsNullOrEmpty(userRole.CompanyCode))
                {
                    existing.CompanyCode = userRole.CompanyCode;
                }
                
                userRole = await _userRoleService.Update(existing);
            }
        }
        else
        {
            userRole.UserCreated = userId;
            
            // If SuperAdmin provides a CompanyCode, use it. Otherwise use their own (which might be null/global).
            if (roles.Contains("SuperAdmin") && !string.IsNullOrEmpty(userRole.CompanyCode))
            {
                 // userRole.CompanyCode is already set from Body
            }
            else
            {
                 // Default to creator's company if not enforcing specific one
                 userRole.CompanyCode = identityUser.CompanyCode; 
            }

            userRole = await _userRoleService.Create(userRole);
        }
        return Ok(userRole);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userRoleService.Delete(id);
        return Ok();
    }
}
