using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface IUserRoleService
{
    Task<IEnumerable<UserRole>> GetAll();
    Task<IEnumerable<UserRole>> GetAll(int userId, List<string> roles);
    Task<UserRole?> GetById(int id);
    Task<UserRole> Create(UserRole role);
    Task<UserRole> Update(UserRole role);
    Task Delete(int id);
}
