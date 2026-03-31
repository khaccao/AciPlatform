using AciPlatform.Domain.Entities.HoSoNhanSu;

namespace AciPlatform.Application.Interfaces.HoSoNhanSu;

public interface IUserCompanyService
{
    Task<List<string>> GetCompanyCodesByUsername(string username);
    Task<UserCompany> CreateAsync(int userId, string companyCode);
    Task<bool> ExistsAsync(int userId, string companyCode);
    Task ClearAsync(int userId);
}
