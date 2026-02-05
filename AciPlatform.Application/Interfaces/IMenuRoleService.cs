using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface IMenuRoleService
{
    Task<IEnumerable<MenuRole>> GetByRoleId(int roleId);
    Task UpdatePermissions(int roleId, List<MenuRole> permissions);
}
