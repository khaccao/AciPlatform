using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities;

namespace AciPlatform.Application.Interfaces;

public interface IMenuService
{
    Task<IEnumerable<Menu>> GetAll();
    Task<object> GetAll(int page, int pageSize, string? searchText, bool? isParent, string? codeParent, List<string> roles, int userId, int? userRoleId);
    Task<IEnumerable<Menu>> GetAll(bool? isParent);
    Task<IEnumerable<Menu>> GetMenusByUserId(int userId);
    Task<IEnumerable<MenuPermissionDto>> GetMenuPermissionsByUserId(int userId);
    Task<MenuPermissionDto?> CheckRole(string menuCode, List<string> roleCodes);
    Task<Menu?> GetById(int id);
    Task<Menu?> GetById(int id, List<string> roles, int userId);
    Task Create(Menu menu);
    Task Create(MenuViewModel menu);
    Task Update(Menu menu);
    Task Update(MenuViewModel menu, List<string> roles, int userId);
    Task Delete(int id);
}
