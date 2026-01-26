using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.LuongPhucLoi;

namespace AciPlatform.Application.Interfaces.LuongPhucLoi;

public interface IAllowanceService
{
    Task<IEnumerable<Allowance>> GetAllAsync();
    Task<Allowance?> GetByIdAsync(int id);
    Task<Allowance> CreateAsync(AllowanceRequest request);
    Task UpdateAsync(int id, AllowanceRequest request);
    Task DeleteAsync(int id);
}

public interface IAllowanceUserService
{
    Task<IEnumerable<AllowanceUser>> GetByUserAsync(int userId);
    Task<AllowanceUser?> GetByIdAsync(int id);
    Task<AllowanceUser> CreateAsync(AllowanceUserRequest request);
    Task UpdateAsync(int id, AllowanceUserRequest request);
    Task DeleteAsync(int id);
}

public interface ISalaryTypeService
{
    Task<IEnumerable<SalaryType>> GetAllAsync();
    Task<SalaryType?> GetByIdAsync(int id);
    Task<SalaryType> CreateAsync(SalaryTypeRequest request);
    Task UpdateAsync(int id, SalaryTypeRequest request);
    Task DeleteAsync(int id);
}
