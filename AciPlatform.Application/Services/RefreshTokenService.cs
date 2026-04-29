using System.Security.Cryptography;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IApplicationDbContext _context;
    private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();

    public RefreshTokenService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> CreateAsync(int userId, TimeSpan lifetime)
    {
        var token = new RefreshToken
        {
            UserId = userId,
            Token = GenerateTokenString(),
            ExpiresAt = DateTime.UtcNow.Add(lifetime),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken?> GetValidTokenAsync(string token)
    {
        var rt = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == token);
        if (rt == null) return null;
        if (rt.IsExpired || rt.IsRevoked) return null;
        return rt;
    }

    public async Task RevokeAsync(RefreshToken token)
    {
        token.RevokedAt = DateTime.UtcNow;
        _context.RefreshTokens.Update(token);
        await _context.SaveChangesAsync();
    }

    private string GenerateTokenString()
    {
        var bytes = new byte[64];
        _rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}

