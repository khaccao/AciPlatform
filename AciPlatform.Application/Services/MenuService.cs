using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class MenuService : IMenuService
{
    private readonly IApplicationDbContext _context;
    private const string SuperAdminRoleCode = "SuperAdmin";

    public MenuService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Menu>> GetAll()
    {
        return await _context.Menus.OrderBy(x => x.Order).ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusByUserId(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null || string.IsNullOrEmpty(user.UserRoleIds))
            return new List<Menu>();

        var roleIds = user.UserRoleIds.Split(',').Select(int.Parse).ToList();

        // Join MenuRoles and Menus
        // Logic: Get menus where UserRole is in user's roles AND View permission is true
        var menus = await (from mr in _context.MenuRoles
                           join m in _context.Menus on mr.MenuId equals m.Id
                           where roleIds.Contains(mr.UserRoleId ?? 0) && mr.View == true
                           select m)
                           .Distinct()
                           .OrderBy(x => x.Order)
                           .ToListAsync();

        return menus;
    }

    public async Task<IEnumerable<MenuPermissionDto>> GetMenuPermissionsByUserId(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null || string.IsNullOrEmpty(user.UserRoleIds))
            return new List<MenuPermissionDto>();

        var roleIds = user.UserRoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.TryParse(id, out var parsed) ? parsed : 0)
            .Where(id => id > 0)
            .ToList();

        if (roleIds.Count == 0)
            return new List<MenuPermissionDto>();

        var roleCodes = await _context.UserRoles
            .Where(r => roleIds.Contains(r.Id))
            .Select(r => r.Code)
            .ToListAsync();

        // Check for UserMenu assignments (higher priority)
        var userMenus = await _context.UserMenus
            .Where(um => um.UserId == userId)
            .Include(um => um.Menu)
            .ToListAsync();

        if (userMenus.Any())
        {
            // User has specific menu assignments - return those
            return userMenus.Select(um => new MenuPermissionDto
            {
                Id = um.MenuId,
                MenuCode = um.MenuCode ?? um.Menu?.Code ?? string.Empty,
                Name = um.Menu?.Name,
                NameEN = um.Menu?.NameEN,
                NameKO = um.Menu?.NameKO,
                Order = um.Menu?.Order,
                View = um.View,
                Add = um.Add,
                Edit = um.Edit,
                Delete = um.Delete
            }).OrderBy(m => m.Order).ToList();
        }

        // Fall back to role-based permissions
        if (roleCodes.Contains(SuperAdminRoleCode))
        {
            var allMenus = await _context.Menus.OrderBy(x => x.Order).ToListAsync();
            return allMenus.Select(menu => new MenuPermissionDto
            {
                Id = menu.Id,
                MenuCode = menu.Code,
                Name = menu.Name,
                NameEN = menu.NameEN,
                NameKO = menu.NameKO,
                Order = menu.Order,
                View = true,
                Add = true,
                Edit = true,
                Delete = true
            }).ToList();
        }

        var menuRoles = await _context.MenuRoles
            .Where(mr => mr.UserRoleId.HasValue && roleIds.Contains(mr.UserRoleId.Value))
            .ToListAsync();

        var menuIds = menuRoles
            .Where(mr => mr.MenuId.HasValue)
            .Select(mr => mr.MenuId!.Value)
            .Distinct()
            .ToList();

        var menus = await _context.Menus
            .Where(m => menuIds.Contains(m.Id))
            .OrderBy(m => m.Order)
            .ToListAsync();

        var menuPermissions = menuRoles
            .GroupBy(mr => mr.MenuId ?? 0)
            .ToDictionary(
                g => g.Key,
                g => new MenuPermissionDto
                {
                    Id = g.Key,
                    MenuCode = g.FirstOrDefault()?.MenuCode ?? string.Empty,
                    View = g.Any(x => x.View == true),
                    Add = g.Any(x => x.Add == true),
                    Edit = g.Any(x => x.Edit == true),
                    Delete = g.Any(x => x.Delete == true)
                });

        foreach (var menu in menus)
        {
            if (menuPermissions.TryGetValue(menu.Id, out var permission))
            {
                permission.MenuCode = menu.Code;
                permission.Name = menu.Name;
                permission.NameEN = menu.NameEN;
                permission.NameKO = menu.NameKO;
                permission.Order = menu.Order;
            }
        }

        return menuPermissions.Values
            .Where(p => !string.IsNullOrEmpty(p.MenuCode))
            .OrderBy(p => p.Order)
            .ToList();
    }

    public async Task<MenuPermissionDto?> CheckRole(string menuCode, List<string> roleCodes)
    {
        if (string.IsNullOrWhiteSpace(menuCode) || roleCodes.Count == 0)
            return null;

        var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Code == menuCode);
        if (menu == null)
            return null;

        if (roleCodes.Contains(SuperAdminRoleCode))
        {
            return new MenuPermissionDto
            {
                Id = menu.Id,
                MenuCode = menu.Code,
                Name = menu.Name,
                NameEN = menu.NameEN,
                NameKO = menu.NameKO,
                Order = menu.Order,
                View = true,
                Add = true,
                Edit = true,
                Delete = true
            };
        }

        var roleIds = await _context.UserRoles
            .Where(r => roleCodes.Contains(r.Code))
            .Select(r => r.Id)
            .ToListAsync();

        var menuRoles = await _context.MenuRoles
            .Where(mr => mr.MenuId == menu.Id && mr.UserRoleId.HasValue && roleIds.Contains(mr.UserRoleId.Value))
            .ToListAsync();

        return new MenuPermissionDto
        {
            Id = menu.Id,
            MenuCode = menu.Code,
            Name = menu.Name,
            NameEN = menu.NameEN,
            NameKO = menu.NameKO,
            Order = menu.Order,
            View = menuRoles.Any(x => x.View == true),
            Add = menuRoles.Any(x => x.Add == true),
            Edit = menuRoles.Any(x => x.Edit == true),
            Delete = menuRoles.Any(x => x.Delete == true)
        };
    }

    public async Task<Menu?> GetById(int id)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task Create(Menu menu)
    {
        if (await _context.Menus.AnyAsync(x => x.Code == menu.Code))
            throw new Exception($"Menu code {menu.Code} already exists");

        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Menu menu)
    {
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var menu = await _context.Menus.FindAsync(id);
        if (menu != null)
        {
            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<object> GetAll(int page, int pageSize, string? searchText, bool? isParent, string? codeParent, List<string> roles, int userId, int? userRoleId)
    {
        var query = _context.Menus.AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
            query = query.Where(x => x.Name != null && x.Name.Contains(searchText) || x.Code.Contains(searchText));

        if (isParent.HasValue)
            query = query.Where(x => x.IsParent == isParent);

        if (!string.IsNullOrEmpty(codeParent))
            query = query.Where(x => x.CodeParent == codeParent);

        var totalItems = await query.CountAsync();
        var menus = await query
            .OrderBy(x => x.Order)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            TotalItems = totalItems,
            Data = menus,
            PageSize = pageSize,
            CurrentPage = page
        };
    }

    public async Task<IEnumerable<Menu>> GetAll(bool? isParent)
    {
        var query = _context.Menus.AsQueryable();
        if (isParent.HasValue)
            query = query.Where(x => x.IsParent == isParent);
        return await query.OrderBy(x => x.Order).ToListAsync();
    }

    public async Task<Menu?> GetById(int id, List<string> roles, int userId)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task Create(MenuViewModel menu)
    {
        var entity = new Menu
        {
            Code = menu.Code,
            Name = menu.Name,
            NameEN = menu.NameEN,
            NameKO = menu.NameKO,
            CodeParent = menu.CodeParent,
            IsParent = menu.IsParent,
            Order = menu.Order,
            Note = menu.Note
        };

        if (await _context.Menus.AnyAsync(x => x.Code == entity.Code))
            throw new Exception($"Menu code {entity.Code} already exists");

        _context.Menus.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(MenuViewModel menuViewModel, List<string> roles, int userId)
    {
        var menu = await _context.Menus.FindAsync(menuViewModel.Id);
        if (menu == null)
            throw new Exception("Menu not found");

        menu.Code = menuViewModel.Code;
        menu.Name = menuViewModel.Name;
        menu.NameEN = menuViewModel.NameEN;
        menu.NameKO = menuViewModel.NameKO;
        menu.CodeParent = menuViewModel.CodeParent;
        menu.IsParent = menuViewModel.IsParent;
        menu.Order = menuViewModel.Order;
        menu.Note = menuViewModel.Note;

        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();
    }
}
