using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Application.Helpers;
using AciPlatform.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Collections.Generic;

namespace AciPlatform.Api.Controllers;

[Authorize(Roles = "SuperAdmin,ADMINCOMPANY")]
[Route("api/[controller]")]
[ApiController]
public class SecurityController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserCompanyService _userCompanyService;
    private readonly IApplicationDbContext _context;

    public SecurityController(IUserService userService, IUserCompanyService userCompanyService, IApplicationDbContext context)
    {
        _userService = userService;
        _userCompanyService = userCompanyService;
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersForSecurity()
    {
        var identity = HttpContext.GetIdentityUser();
        var roles = identity.Role ?? "[]";
        
        IEnumerable<User> users;
        
        if (roles.Contains("SuperAdmin"))
        {
            users = await _userService.GetAll();
        }
        else
        {
            var companyCodes = await _userCompanyService.GetCompanyCodesByUsername(identity.UserName);
            var primaryCompany = companyCodes.FirstOrDefault();
            
            if (string.IsNullOrEmpty(primaryCompany))
                return Ok(new List<object>());

            var companyUserIds = await _context.UserCompanies
                .Where(uc => uc.CompanyCode == primaryCompany)
                .Select(uc => uc.UserId)
                .ToListAsync();
            
            users = await _context.Users
                .Where(u => companyUserIds.Contains(u.Id) && !u.IsDeleted)
                .ToListAsync();
        }

        return Ok(users.Select(u => new
        {
            u.Id,
            u.Username,
            u.FullName,
            u.Email,
            u.TwoFactorEnabled,
            HasSecret = !string.IsNullOrEmpty(u.TwoFactorSecret)
        }));
    }

    [HttpPost("enable-2fa/{userId}")]
    public async Task<IActionResult> EnableTwoFactor(int userId)
    {
        if (!await CanManageUser(userId))
            return Forbid();

        var result = await _userService.EnableTwoFactor(userId);
        return Ok(result);
    }

    [HttpPost("confirm-2fa/{userId}")]
    public async Task<IActionResult> ConfirmTwoFactor(int userId, [FromBody] TwoFactorRequest request)
    {
        if (!await CanManageUser(userId))
            return Forbid();

        var result = await _userService.ConfirmEnableTwoFactor(userId, request.Code);
        if (result)
            return Ok(true);
        
        return BadRequest(new { message = "Mã OTP không chính xác hoặc đã hết hạn" });
    }

    [HttpPost("disable-2fa/{userId}")]
    public async Task<IActionResult> DisableTwoFactor(int userId)
    {
        if (!await CanManageUser(userId))
            return Forbid();

        var result = await _userService.DisableTwoFactor(userId);
        return Ok(result);
    }

    [HttpGet("setup-2fa/{userId}")]
    public async Task<IActionResult> GetTwoFactorSetup(int userId)
    {
        if (!await CanManageUser(userId))
            return Forbid();

        var result = await _userService.GetTwoFactorSetup(userId);
        return Ok(result);
    }

    private async Task<bool> CanManageUser(int targetUserId)
    {
        var identity = HttpContext.GetIdentityUser();
        var roles = identity.Role ?? "[]";

        if (roles.Contains("SuperAdmin"))
            return true;

        if (roles.Contains("ADMINCOMPANY"))
        {
            var companyCodes = await _userCompanyService.GetCompanyCodesByUsername(identity.UserName);
            var primaryCompany = companyCodes.FirstOrDefault();
            
            if (string.IsNullOrEmpty(primaryCompany))
                return false;

            return await _userCompanyService.ExistsAsync(targetUserId, primaryCompany);
        }

        return false;
    }
}
