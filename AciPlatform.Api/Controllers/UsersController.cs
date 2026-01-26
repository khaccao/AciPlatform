using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Helpers;
using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AciPlatform.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] UserViewModel param)
    {
        string roles = "[]";
        int userId = 0;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
        {
            roles = identity.FindFirst(x => x.Type == "RoleName")?.Value?.ToString() ?? "[]";
            userId = int.Parse(identity.FindFirst(x => x.Type == "UserId")?.Value ?? "0");
        }
        
        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        return Ok(await _userService.GetPaging(new UserFilterParams
        {
            BirthDay = param.Birthday,
            Gender = param.Gender,
            Keyword = param.SearchText,
            PositionId = param.Positionid,
            WarehouseId = param.Warehouseid,
            DepartmentId = param.Departmentid,
            RequestPassword = param.RequestPassword,
            Quit = param.Quit,
            CurrentPage = param.Page,
            PageSize = param.PageSize,
            StartDate = param.StartDate,
            EndDate = param.EndDate,
            TargetId = param.Targetid,
            Month = param.Month,
            DegreeId = param.Degreeid ?? 0,
            CertificateId = param.Certificateid ?? 0,
            Ids = param.Ids,
            roles = listRole,
            UserId = userId
        }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetById(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpGet("get-total-reset-pass")]
    public async Task<IActionResult> GetTotalResetPass()
    {
        var total = await _userService.GetTotalResetPass();
        return Ok(total);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUserRequest model)
    {
        var user = await _userService.GetById(id);
        if (user == null) return NotFound(new { message = "User not found" });

        user.Username = model.Username;
        user.FullName = model.FullName;
        user.Email = model.Email;
        user.Phone = model.Phone;
        user.UserRoleIds = model.UserRoleIds;

        await _userService.Update(user, model.Password);
        return Ok(new { message = "User updated successfully" });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest model)
    {
        var user = new User
        {
            Username = model.Username,
            FullName = model.FullName,
            Email = model.Email,
            Phone = model.Phone,
            UserRoleIds = model.UserRoleIds,
            CreatedDate = DateTime.Now,
            Status = 1
        };

        var createdUser = await _userService.Create(user, model.Password ?? "123456");
        return Ok(new { message = "User created successfully", userId = createdUser.Id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _userService.Delete(id);
        return Ok(new { message = "User deleted successfully" });
    }

    [HttpPost("resetPassword")]
    public async Task<IActionResult> ResetPassword([FromBody] List<int> ids)
    {
        await _userService.ResetPasswordForMultipleUsers(ids, "123456");
        return Ok();
    }

    [HttpPost("getallusername")]
    public async Task<IActionResult> GetAllUserName()
    {
        var usernames = await _userService.GetAllUserName();
        return Ok(usernames);
    }

    [HttpGet("getAllUserActive")]
    public async Task<IActionResult> GetAllUserActive()
    {
        string roles = "[]";
        int userId = 0;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
        {
            roles = identity.FindFirst(x => x.Type == "RoleName")?.Value?.ToString() ?? "[]";
            userId = int.Parse(identity.FindFirst(x => x.Type == "UserId")?.Value ?? "0");
        }
        List<string> listRole = JsonConvert.DeserializeObject<List<string>>(roles) ?? new List<string>();

        var result = await _userService.GetAllUserActive(listRole, userId);
        return Ok(new BaseResponseModel
        {
            Data = result,
        });
    }

    [HttpGet("user-not-roles")]
    public async Task<IActionResult> GetAllUserNotRole()
    {
        var response = await _userService.GetAllUserNotRole();
        return Ok(new BaseResponseCommonModel
        {
            Data = response,
        });
    }

    [HttpPut("update-current-year")]
    public async Task<IActionResult> UpdateCurrentYear(int year)
    {
        int userId = 0;
        if (HttpContext.User.Identity is ClaimsIdentity identity)
        {
            userId = int.Parse(identity.FindFirst(x => x.Type == "UserId")?.Value ?? "0");
        }

        await _userService.UpdateCurrentYear(year, userId);
        return Ok(new BaseResponseModel
        {
            Data = null,
        });
    }
}
