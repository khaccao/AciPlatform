using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface IUserMenuService
{
    Task<IEnumerable<UserMenu>> GetByUserId(int userId);
    Task<IEnumerable<MenuPermissionDto>> GetMenuPermissionsForUser(int userId);
    Task AssignMenusToUser(int userId, List<UserMenuAssignDto> menus, int createdBy);
    Task RemoveMenuFromUser(int userId, int menuId);
    Task ClearUserMenus(int userId);
}
