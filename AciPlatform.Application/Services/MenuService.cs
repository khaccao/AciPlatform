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

        var roleIds = user.UserRoleIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(id => int.TryParse(id.Trim(), out var parsed) ? parsed : 0)
            .Where(id => id > 0)
            .ToList();

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

        // Check for SuperAdmin (highest priority)
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
                Delete = true,
                Approve = true,
                IsParent = menu.IsParent ?? false,
                CodeParent = menu.CodeParent
            }).ToList();
        }

        // Check for UserMenu assignments
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
                Delete = um.Delete,
                Approve = um.Approve,
                IsParent = um.Menu?.IsParent ?? false,
                CodeParent = um.Menu?.CodeParent
            }).OrderBy(m => m.Order).ToList();
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
                    Delete = g.Any(x => x.Delete == true),
                    Approve = g.Any(x => x.Approve == true),
                    IsParent = false, // Will be updated below
                    CodeParent = null // Will be updated below
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
                permission.IsParent = menu.IsParent ?? false;
                permission.CodeParent = menu.CodeParent;
            }
        }

        // Logic to auto-include Parent Menus
        var authorizedMenuCodes = menuPermissions.Values.Select(p => p.MenuCode).ToHashSet();
        var authorizedMenuParents = menus.Where(m => authorizedMenuCodes.Contains(m.Code)).Select(m => m.CodeParent).Distinct().ToList();

        // Find parents that are not yet in the authorized list
        var missingParents = await _context.Menus
            .Where(m => authorizedMenuParents.Contains(m.Code) && !authorizedMenuCodes.Contains(m.Code))
            .ToListAsync();
            
        // Add permissions for missing parents (View only)
        foreach (var parent in missingParents)
        {
             if (!menuPermissions.ContainsKey(parent.Id))
             {
                 menuPermissions[parent.Id] = new MenuPermissionDto
                 {
                     Id = parent.Id,
                     MenuCode = parent.Code,
                     Name = parent.Name,
                     NameEN = parent.NameEN,
                     NameKO = parent.NameKO,
                     Order = parent.Order,
                     View = true, // Force View for parent
                     Add = false,
                     Edit = false,
                     Delete = false,
                     Approve = false,
                     IsParent = parent.IsParent ?? false,
                     CodeParent = parent.CodeParent
                 };
                 // Also add to the list for final processing if needed, 
                 // though we are iterating 'menus' below which needs to include these new ones if we want them sorted correctly.
                 // Better approach: Add to 'menus' list so the final sorting loop works.
                 menus.Add(parent);
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
                Delete = true,
                Approve = true
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
            Delete = menuRoles.Any(x => x.Delete == true),
            Approve = menuRoles.Any(x => x.Approve == true)
        };
    }

    public async Task<Menu?> GetById(int id)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task Create(Menu menu)
    {
        if (await _context.Menus.AnyAsync(x => x.Code == menu.Code))
            throw new ArgumentException($"Menu code {menu.Code} already exists");

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
            CodeParent = string.IsNullOrEmpty(menu.CodeParent) ? null : menu.CodeParent,
            IsParent = menu.IsParent,
            Order = menu.Order,
            Note = menu.Note
        };

        if (await _context.Menus.AnyAsync(x => x.Code == entity.Code))
            throw new ArgumentException($"Mã Menu '{entity.Code}' đã tồn tại trong hệ thống.");

        _context.Menus.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(MenuViewModel menuViewModel, List<string> roles, int userId)
    {
        var menu = await _context.Menus.FindAsync(menuViewModel.Id);
        if (menu == null)
            throw new Exception("Menu not found");

        var oldCode = menu.Code;
        var newCode = menuViewModel.Code?.Trim() ?? string.Empty;
        var newCodeParent = string.IsNullOrWhiteSpace(menuViewModel.CodeParent) ? null : menuViewModel.CodeParent.Trim();

        // 1. Prevent self-parenting
        if (newCode == newCodeParent && !string.IsNullOrEmpty(newCode))
        {
            throw new ArgumentException("Menu không thể làm danh mục cha của chính nó.");
        }

        // 2. Validate code uniqueness if code changed
        if (oldCode != newCode)
        {
            if (await _context.Menus.AnyAsync(x => x.Id != menuViewModel.Id && x.Code == newCode))
                throw new ArgumentException($"Mã Menu '{newCode}' đã được sử dụng bởi một Menu khác.");
        }

        // 3. Update the menu
        if (oldCode != newCode)
        {
            menu.Code = newCode;
        }
        
        menu.Name = menuViewModel.Name;
        menu.NameEN = menuViewModel.NameEN;
        menu.NameKO = menuViewModel.NameKO;
        menu.CodeParent = newCodeParent;
        menu.IsParent = menuViewModel.IsParent;
        menu.Order = menuViewModel.Order;
        menu.Note = menuViewModel.Note;

        _context.Menus.Update(menu);

        // 4. If Code changed, update children's CodeParent reference
        if (oldCode != newCode)
        {
            var children = await _context.Menus.Where(m => m.CodeParent == oldCode).ToListAsync();
            foreach (var child in children)
            {
                child.CodeParent = newCode;
            }
        }

        await _context.SaveChangesAsync();
    }
}
