using AciPlatform.Application.Helpers;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Constants;
using AciPlatform.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using AciPlatform.Application.Interfaces.HoSoNhanSu;

namespace AciPlatform.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IMenuService _menuService;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;
    private readonly ITokenService _tokenService;
    private readonly IInvoiceAuthorize _invoiceAuthorize;
    private readonly IUserCompanyService _userCompanyService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthController(
        IUserService userService, 
        IUserRoleService userRoleService, 
        IMenuService menuService, 
        IConfiguration configuration, 
        IApplicationDbContext context,
        ITokenService tokenService,
        IInvoiceAuthorize invoiceAuthorize,
        IUserCompanyService userCompanyService,
        IRefreshTokenService refreshTokenService)
    {
        _userService = userService;
        _userRoleService = userRoleService;
        _menuService = menuService;
        _configuration = configuration;
        _context = context;
        _tokenService = tokenService;
        _invoiceAuthorize = invoiceAuthorize;
        _userCompanyService = userCompanyService;
        _refreshTokenService = refreshTokenService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] AuthenticateModel model)
    {
        try
        {
            var userName = model.Username?.Trim();
            var password = model.Password?.Trim();

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return Ok(new ObjectReturn
                {
                    message = ResultErrorConstants.USER_IS_NOT_EXIST,
                    status = (int)ErrorEnum.USER_IS_NOT_EXIST
                });
            }

            var user = await _userService.GetByUserName(userName);
            if (user == null)
            {
                return Ok(new ObjectReturn
                {
                    message = ResultErrorConstants.USER_IS_NOT_EXIST,
                    status = (int)ErrorEnum.USER_IS_NOT_EXIST
                });
            }

            var checkUser = await _userService.Authenticate(userName, password);
            if (checkUser == null)
            {
                return Ok(new ObjectReturn
                {
                    message = ResultErrorConstants.ERROR_PASS,
                    status = (int)ErrorEnum.ERROR_PASS
                });
            }

            // CompanyCode enforcement (if provided)
            if (!string.IsNullOrWhiteSpace(model.CompanyCode))
            {
                var hasCompany = await _userCompanyService.ExistsAsync(user.Id, model.CompanyCode);
                if (!hasCompany)
                {
                    return Ok(new ObjectReturn
                    {
                        message = "CompanyCode is invalid for this user",
                        status = (int)ErrorEnum.USER_IS_NOT_EXIST
                    });
                }
            }

            await _userService.UpdateLastLogin(user.Id);

            var roleIds = user.UserRoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
            var roles = (await _userRoleService.GetAll())
                .Where(o => roleIds.Contains(o.Id.ToString()))
                .Select(x => x.Code)
                .ToList();

            var tokenString = _tokenService.GenerateToken(user, roles); // TokenService needs to accept User (not UserAuthDto) or I map it. 
            // checkUser is UserAuthDto. user is User. 
            // _tokenService.GenerateToken(user, roles) uses User entity. 
            // Note: CodeMau Authenticate returned UserMapper.Auth (UserAuthDto). 
            // But I retrieved `user` entity above using GetByUserName. 
            // So passing `user` (Entity) to TokenService is fine.

            var menus = await _menuService.GetMenuPermissionsByUserId(user.Id);

            var refresh = await _refreshTokenService.CreateAsync(user.Id, TimeSpan.FromDays(30));

            return Ok(new ObjectReturn
            {
                data = new
                {
                    Id = user.Id,
                    Username = user.Username,
                    Fullname = user.FullName,
                    Avatar = user.Avatar,
                    Timekeeper = user.Timekeeper,
                    Token = tokenString,
                    RefreshToken = refresh.Token,
                    TargetId = user.TargetId,
                    RoleName = roles,
                    Menus = menus,
                    UserRoleIds = user.UserRoleIds,
                    YearCurrent = user.YearCurrent,
                    IsDark = false, 
                    Theme = "",
                },
                status = 200,
                message = ResultErrorConstants.LOGIN_SUCCESS,
            });
        }
        catch (Exception ex)
        {
            return Ok(new ObjectReturn
            {
                message = ex.Message,
                status = 400
            });
        }
    }

    [HttpPost("requestForgotPass")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestForgotPass([FromBody] ForgotPasswordRequest model)
    {
        if (!String.IsNullOrEmpty(model.Username))
        {
            var user = await _userService.GetByUserName(model.Username);
            if (user != null)
            {
                await _userService.RequestPasswordReset(model.Username);
                return Ok(true);
            }
        }

        return Ok(false);
    }

    [HttpPut("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest model)
    {
        if (model.Id == 0)
        {
            model.Id = HttpContext.GetIdentityUser().Id;
        }

        // check is current user
        if (!await _userService.CheckPassword(model.Id, model.OldPassword))
        {
            return Ok(new ObjectReturn
            {
                message = ErrorEnum.ERROR_PASS.ToString(),
                status = Convert.ToInt32(ErrorEnum.ERROR_PASS)
            });
        }

        await _userService.UpdatePassword(model.Id, model.NewPassword);
        return Ok(new ObjectReturn
        {
            data = ErrorEnum.SUCCESS,
            status = Convert.ToInt32(ErrorEnum.SUCCESS)
        });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.Password))
            return BadRequest(new { message = "Username and password are required" });

        var user = new User
        {
            Username = model.Username.Trim(),
            FullName = model.FullName,
            Email = model.Email,
            UserRoleIds = model.UserRoleIds
        };

        try
        {
            await _userService.Create(user, model.Password);
            if (!string.IsNullOrWhiteSpace(model.CompanyCode))
            {
                await _userCompanyService.CreateAsync(user.Id, model.CompanyCode);
            }
            return Ok(new { message = "User registered successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.RefreshToken))
            return BadRequest(new { message = "RefreshToken is required" });

        var rt = await _refreshTokenService.GetValidTokenAsync(model.RefreshToken);
        if (rt == null)
            return Unauthorized(new { message = "Invalid or expired refresh token" });

        var user = await _userService.GetById(rt.UserId);
        if (user == null)
            return Unauthorized(new { message = "User not found" });

        var roleIds = user.UserRoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();
        var roles = (await _userRoleService.GetAll())
            .Where(o => roleIds.Contains(o.Id.ToString()))
            .Select(x => x.Code)
            .ToList();

        var newAccessToken = _tokenService.GenerateToken(user, roles);
        await _refreshTokenService.RevokeAsync(rt);
        var newRefresh = await _refreshTokenService.CreateAsync(user.Id, TimeSpan.FromDays(30));

        return Ok(new
        {
            token = newAccessToken,
            refreshToken = newRefresh.Token
        });
    }

    [HttpGet("username-check")]
    [AllowAnonymous]
    public async Task<IActionResult> UsernameCheck([FromQuery] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            return BadRequest(new { message = "username is required" });

        var companyCodes = await _userCompanyService.GetCompanyCodesByUsername(username.Trim());
        if (!companyCodes.Any())
            return NotFound(new { message = "User not found or no companies" });

        return Ok(new { username = username.Trim(), companyCodes });
    }

    [HttpPost("guess-login")]
    [AllowAnonymous]
    public IActionResult GuessLogin()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var authClaims = new List<Claim>
                {
                    new("UserId", "0"),
                    new(ClaimTypes.Name, "guess"),
                    new("FullName",  "Guess"),
                    new("RoleName", "[]"),
                };

        var token = GetToken(authClaims);

        var tokenString = tokenHandler.WriteToken(token);

        // return basic user info and authentication token
        return Ok(
                new ObjectReturn
                {
                    data = new
                    {
                        Id = 0,
                        Username = "Guess",
                        Fullname = "Guess",
                        Avatar = "",
                        Timekeeper = "",
                        Token = tokenString,
                        TargetId = "",
                        RoleName = "[]",
                        Menus = "[]",
                        UserRoleIds = "",
                        IsDark = "",
                        Theme = "",
                        YearCurrent = DateTime.Now.Year,
                    },
                    status = 200,
                    message = ResultErrorConstants.LOGIN_SUCCESS,
                });
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? "your-secret-key-here"));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(8),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }

    [HttpPost("invoice")]
    [AllowAnonymous]
    public async Task<IActionResult> Invoice(int billId)
    {
        await _invoiceAuthorize.CreateInvoice(billId);
        return Ok(true);
    }

    [HttpPost("seed")]
    [AllowAnonymous]
    public async Task<IActionResult> Seed()
    {
        try
        {
            // Delete all existing users first to avoid hash mismatch issues
            var existingUsers = await _context.Users.ToListAsync();
            if (existingUsers.Any())
            {
                _context.Users.RemoveRange(existingUsers);
                await _context.SaveChangesAsync();
            }

            // 1. Seed Roles
            if (!(await _userRoleService.GetAll()).Any())
            {
                await _userRoleService.Create(new UserRole { Code = "SuperAdmin", Title = "Super Admin", Order = 1 });
                await _userRoleService.Create(new UserRole { Code = "Staff", Title = "Staff", Order = 2 });
            }
            
            // 2. Seed User (always create fresh)
            var admin = new User 
            { 
                Username = "admin", 
                FullName = "Administrator", 
                Email = "admin@aci.com",
                UserRoleIds = "1", // Assuming ID 1 is SuperAdmin based on creation order
                CreatedDate = DateTime.Now,
                Status = 1
            };
            await _userService.Create(admin, "admin");

            // 3. Seed Menus
            if (!(await _menuService.GetAll()).Any())
            {
                await _menuService.Create(new Menu { Code = "users", Name = "User Management", Order = 1 });
                await _menuService.Create(new Menu { Code = "menus", Name = "Menu Management", Order = 2 });
            }

            // Verify user was created correctly
            var createdUser = await _userService.GetByUserName("admin");
            return Ok(new 
            { 
                message = "Database seeded with admin/admin",
                userCreated = createdUser != null,
                passwordHashLength = createdUser?.PasswordHash.Length,
                passwordSaltLength = createdUser?.PasswordSalt.Length
            });
        }
        catch (Exception ex)
        {
            return Ok(new 
            { 
                status = 400,
                message = $"Seed failed: {ex.Message}",
                stackTrace = ex.StackTrace
            });
        }
    }

    [HttpGet("debug-db")]
    [AllowAnonymous]
    public async Task<IActionResult> DebugDb()
    {
        var dbContext = _context as Microsoft.EntityFrameworkCore.DbContext;
        var connStr = dbContext?.Database.GetDbConnection().ConnectionString;
        
        var tables = new List<string>();
        try 
        {
            var connection = dbContext?.Database.GetDbConnection();
            if (connection != null)
            {
                await connection.OpenAsync();
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
                using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    tables.Add(reader.GetString(0));
                }
            }
        }
        catch(Exception ex)
        {
            tables.Add($"Error: {ex.Message}");
        }

        var users = await _context.Users.Select(u => new 
        {
            u.Id,
            u.Username,
            u.FullName,
            PasswordHashLength = u.PasswordHash.Length,
            PasswordSaltLength = u.PasswordSalt.Length
        }).ToListAsync();

        return Ok(new 
        { 
            ActiveConnectionString = connStr,
            Tables = tables,
            Users = users,
            UserCount = users.Count
        });
    }
}

