using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces.HoSoNhanSu;

public interface ICompanyService
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByCodeAsync(string code);
}
