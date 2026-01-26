using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user, List<string> roles);
}
