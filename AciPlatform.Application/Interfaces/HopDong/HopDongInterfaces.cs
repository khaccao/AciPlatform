using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.HopDong;

namespace AciPlatform.Application.Interfaces.HopDong;

public interface IContractTypeService
{
    Task<IEnumerable<ContractType>> GetAllAsync();
    Task<ContractType?> GetByIdAsync(int id);
    Task<ContractType> CreateAsync(ContractTypeRequest request);
    Task UpdateAsync(int id, ContractTypeRequest request);
    Task DeleteAsync(int id);
}

public interface IContractFileService
{
    Task<IEnumerable<ContractFile>> GetAllAsync();
    Task<ContractFile?> GetByIdAsync(int id);
    Task<ContractFile> CreateAsync(ContractFileRequest request);
    Task UpdateAsync(int id, ContractFileRequest request);
    Task DeleteAsync(int id);
}

public interface IUserContractHistoryService
{
    Task<IEnumerable<UserContractHistory>> GetAllAsync();
    Task<IEnumerable<UserContractHistory>> GetByUserAsync(int userId);
    Task<UserContractHistory?> GetByIdAsync(int id);
    Task<UserContractHistory> CreateAsync(UserContractHistoryRequest request);
    Task UpdateAsync(int id, UserContractHistoryRequest request);
    Task DeleteAsync(int id);
}
