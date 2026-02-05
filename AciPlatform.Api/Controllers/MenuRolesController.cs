using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers;

[Authorize(Roles = "SuperAdmin,ADMINCOMPANY")]
[Route("api/[controller]")]
[ApiController]
public class MenuRolesController : ControllerBase
{
    private readonly IMenuRoleService _menuRoleService;

    public MenuRolesController(IMenuRoleService menuRoleService)
    {
        _menuRoleService = menuRoleService;
    }

    [HttpGet("role/{roleId}")]
    public async Task<IActionResult> GetByRoleId(int roleId)
    {
        var data = await _menuRoleService.GetByRoleId(roleId);
        return Ok(data);
    }

    [HttpPost("role/{roleId}")]
    public async Task<IActionResult> UpdatePermissions(int roleId, [FromBody] List<MenuRole> permissions)
    {
        await _menuRoleService.UpdatePermissions(roleId, permissions);
        return Ok();
    }
}
