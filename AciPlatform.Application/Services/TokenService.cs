using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AciPlatform.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _context;

    public TokenService(IConfiguration configuration, IApplicationDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    public string GenerateToken(User user, List<string> roles, string? companyCode = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is not configured"));
        
        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FullName", user.FullName ?? ""),
            new Claim("RoleName", JsonSerializer.Serialize(roles)),
            new Claim("CompanyCode", companyCode ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        // Add roles as standard Role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpireHours"] ?? "8")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}

