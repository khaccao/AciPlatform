using AciPlatform.Application.DTOs;
using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserMenusController : ControllerBase
{
    private readonly IUserMenuService _userMenuService;

    public UserMenusController(IUserMenuService userMenuService)
    {
        _userMenuService = userMenuService;
    }

    /// <summary>
    /// Lấy danh sách menu được assign cho user
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var menus = await _userMenuService.GetByUserId(userId);
        return Ok(new ObjectReturn
        {
            data = menus,
            status = 200,
            message = "Success"
        });
    }

    /// <summary>
    /// Lấy danh sách menu permissions cho user
    /// </summary>
    [HttpGet("{userId}/permissions")]
    public async Task<IActionResult> GetMenuPermissions(int userId)
    {
        var permissions = await _userMenuService.GetMenuPermissionsForUser(userId);
        return Ok(new ObjectReturn
        {
            data = permissions,
            status = 200,
            message = "Success"
        });
    }

    /// <summary>
    /// Assign menus cho user
    /// </summary>
    [HttpPost("{userId}/assign")]
    public async Task<IActionResult> AssignMenus(int userId, [FromBody] List<UserMenuAssignDto> menus)
    {
        try
        {
            var currentUser = HttpContext.GetIdentityUser();
            await _userMenuService.AssignMenusToUser(userId, menus, currentUser.Id);
            
            return Ok(new ObjectReturn
            {
                status = 200,
                message = "Menus assigned successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ObjectReturn
            {
                status = 400,
                message = ex.Message
            });
        }
    }

    /// <summary>
    /// Xóa menu assignment của user
    /// </summary>
    [HttpDelete("{userId}/menu/{menuId}")]
    public async Task<IActionResult> RemoveMenu(int userId, int menuId)
    {
        await _userMenuService.RemoveMenuFromUser(userId, menuId);
        
        return Ok(new ObjectReturn
        {
            status = 200,
            message = "Menu removed successfully"
        });
    }

    /// <summary>
    /// Xóa tất cả menu assignments của user
    /// </summary>
    [HttpDelete("{userId}/clear")]
    public async Task<IActionResult> ClearMenus(int userId)
    {
        await _userMenuService.ClearUserMenus(userId);
        
        return Ok(new ObjectReturn
        {
            status = 200,
            message = "All menus cleared successfully"
        });
    }
}
