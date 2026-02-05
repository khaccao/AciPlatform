using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using AciPlatform.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Newtonsoft.Json;

namespace AciPlatform.Api.Controllers;

[Authorize(Roles = "SuperAdmin,ADMINCOMPANY")]
[Route("api/[controller]")]
[ApiController]
public class MenusController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenusController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] MenuPagingationRequestModel param)
    {
        var identityUser = HttpContext.GetIdentityUser();
        int userId = identityUser.Id;
        string roles = identityUser.Role ?? "[]";

        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        return Ok(await _menuService.GetAll(param.Page, param.PageSize, param.SearchText, param.isParent, param.CodeParent, listRole, userId, param.userRoleId));
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList([FromQuery] MenuPagingationRequestModel param)
    {
        var identityUser = HttpContext.GetIdentityUser();
        string roles = identityUser.Role ?? "[]";
        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        var results = await _menuService.GetAll(param.isParent);
        
        // If not SuperAdmin, filter results to only include menus the current user has access to
        if (!listRole.Contains("SuperAdmin"))
        {
             // Get permissions for current user
             var userPermissions = await _menuService.GetMenuPermissionsByUserId(identityUser.Id);
             var allowedMenuIds = userPermissions.Where(p => p.View).Select(p => p.Id).ToHashSet();
             
             results = results.Where(m => allowedMenuIds.Contains(m.Id));
        }

        return Ok(new BaseResponseCommonModel
        {
            Data = results,
        });
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var menus = await _menuService.GetMenusByUserId(userId);
        return Ok(menus);
    }

    [HttpGet("user/{userId}/permissions")]
    public async Task<IActionResult> GetPermissionsByUserId(int userId)
    {
        var menus = await _menuService.GetMenuPermissionsByUserId(userId);
        return Ok(menus);
    }

    [HttpGet("check-role")]
    public async Task<IActionResult> CheckRole([FromQuery] string menuCode)
    {
        var identityUser = HttpContext.GetIdentityUser();
        string roles = identityUser.Role ?? "[]";
        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        var result = await _menuService.CheckRole(menuCode, listRole);
        return Ok(new ObjectReturn
        {
            status = 200,
            data = result,
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var identityUser = HttpContext.GetIdentityUser();
        int userId = identityUser.Id;
        string roles = identityUser.Role ?? "[]";

        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        var model = await _menuService.GetById(id, listRole, userId);
        return Ok(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MenuViewModel model)
    {
        await _menuService.Create(model);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MenuViewModel model)
    {
        var identityUser = HttpContext.GetIdentityUser();
        int userId = identityUser.Id;
        string roles = identityUser.Role ?? "[]";

        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        await _menuService.Update(model, listRole, userId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _menuService.Delete(id);
        return Ok(new { message = "Menu deleted successfully" });
    }
}
