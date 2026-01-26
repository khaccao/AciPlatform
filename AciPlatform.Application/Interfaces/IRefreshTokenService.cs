using AciPlatform.Domain.Entities.Auth;

namespace AciPlatform.Application.Interfaces;

public interface IRefreshTokenService
{
    Task<RefreshToken> CreateAsync(int userId, TimeSpan lifetime);
    Task<RefreshToken?> GetValidTokenAsync(string token);
    Task RevokeAsync(RefreshToken token);
}
