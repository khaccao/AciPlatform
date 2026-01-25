using AciPlatform.Application.Interfaces;
using AciPlatform.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AciPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WebAuthController : ControllerBase
{
    private readonly IWebAuthService _webAuthService;

    public WebAuthController(IWebAuthService webAuthService)
    {
        _webAuthService = webAuthService;
    }

    /// <summary>
    /// Đăng nhập web
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthenticateModel model)
    {
        var user = await _webAuthService.Authenticate(model.Username ?? string.Empty, model.Password ?? string.Empty);

        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var authClaims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.Phone ?? string.Empty),
                    new Claim("Name",  !String.IsNullOrEmpty(user.Name)? user.Name : ""),
                };

        var tokenString = _webAuthService.GenerateToken(authClaims);

        return Ok(new
        {
            Id = user.Id,
            Username = user.Code,
            Fullname = user.Name,
            Avatar = user.Avatar,
            Token = tokenString,
            Email = user.Email,
            Phone = user.Phone,
        });
    }

    /// <summary>
    /// Đăng nhập web social
    /// </summary>
    [HttpPost("loginsocial")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginSocial(AuthenticateSocialModel model)
    {
        var user = await _webAuthService.RegisterAccountSocial(model);
        var authClaims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.Phone ?? string.Empty),
                    new Claim("Name",  !String.IsNullOrEmpty(user.Name)? user.Name : ""),
                };

        var tokenString = _webAuthService.GenerateToken(authClaims);

        return Ok(new
        {
            Id = user.Id,
            Username = user.Code,
            Fullname = user.Name,
            Avatar = user.Avatar,
            Token = tokenString,
            Email = user.Email,
            Phone = user.Phone,
            IsLoginSocial = true
        });
    }

    /// <summary>
    /// Đăng ký tài khoản
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(WebCustomerV2Model model)
    {
        var user = await _webAuthService.Register(model);
        var authClaims = new List<Claim>
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim("UserName", user.Phone ?? string.Empty),
                    new Claim("Name",  !String.IsNullOrEmpty(user.Name)? user.Name : ""),
                };

        var tokenString = _webAuthService.GenerateToken(authClaims);

        return Ok(new ObjectReturn
        {
            status = 200,
            data = new
            {
                Id = user.Id,
                Code = user.Code,
                Name = user.Name,
                Avatar = user.Avatar,
                Phone = user.Phone,
                Token = tokenString,
            }
        });
    }

    /// <summary>
    /// Update email
    /// </summary>
    [HttpPost("update-email")]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateEmail(WebCustomerV2Model model)
    {
        await _webAuthService.UpdateMail(model);
        return Ok();
    }

    [HttpGet("info/{id}")]
    [Authorize]
    public async Task<IActionResult> GetCustomerAsync(int id)
    {
        var response = await _webAuthService.GetCustomerAsync(id);
        return Ok(response);
    }

    [HttpPost("info")]
    [Authorize]
    public async Task<IActionResult> UpdateInfo([FromBody] WebCustomerUpdateModel model)
    {
        await _webAuthService.UpdateCustomerAsync(model);
        return Ok();
    }

    [HttpPost("change-pass-word/{id}")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(int id, string password)
    {
        await _webAuthService.ChangePassWordCustomerAsync(id, password);
        return Ok();
    }
}
