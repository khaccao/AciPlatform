using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using AciPlatform.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class MenuService : IMenuService
{
    private readonly IApplicationDbContext _context;

    public MenuService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Menu>> GetAll()
    {
        return await _context.Menus.ToListAsync();
    }

    public async Task<object> GetAll(int page, int pageSize, string? searchText, bool? isParent, string? codeParent, List<string> roles, int userId, int? userRoleId)
    {
        var query = _context.Menus.AsQueryable();

        if (!string.IsNullOrEmpty(searchText))
        {
            query = query.Where(x => (x.Name != null && x.Name.Contains(searchText)) || (x.Code != null && x.Code.Contains(searchText)));
        }

        if (isParent.HasValue)
        {
            query = query.Where(x => x.IsParent == isParent.Value);
        }

        if (!string.IsNullOrEmpty(codeParent))
        {
            query = query.Where(x => x.CodeParent == codeParent);
        }

        var total = await query.CountAsync();
        var data = await query.OrderBy(x => x.Order).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        return new { Data = data, TotalItems = total, CurrentPage = page, PageSize = pageSize };
    }

    public async Task<IEnumerable<Menu>> GetAll(bool? isParent)
    {
        var query = _context.Menus.AsQueryable();
        if (isParent.HasValue)
        {
            query = query.Where(x => x.IsParent == isParent.Value);
        }
        return await query.OrderBy(x => x.Order).ToListAsync();
    }

    public async Task<IEnumerable<Menu>> GetMenusByUserId(int userId)
    {
        return await _context.Menus.ToListAsync(); // Basic implementation
    }

    public async Task<IEnumerable<MenuPermissionDto>> GetMenuPermissionsByUserId(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Enumerable.Empty<MenuPermissionDto>();

        var roleIds = user.UserRoleIds?.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Where(s => int.TryParse(s, out _))
            .Select(int.Parse).ToList() ?? new List<int>();

        var roles = await _context.UserRoles.Where(r => roleIds.Contains(r.Id)).ToListAsync();
        var isSuperAdmin = roles.Any(r => r.Code == "SuperAdmin");

        // DEBUG LOG
        try {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "permission_debug.log");
            var roleCodes = string.Join(", ", roles.Select(r => r.Code));
            File.AppendAllText(logPath, $"[{DateTime.Now}] User: {userId}, Roles: {roleCodes}, isSuperAdmin: {isSuperAdmin}\n");
        } catch {}

        var allMenus = await _context.Menus.OrderBy(x => x.Order).ToListAsync();

        if (isSuperAdmin)
        {
            return allMenus.Select(x => new MenuPermissionDto
            {
                Id = x.Id,
                MenuCode = x.Code,
                Name = x.Name,
                NameEN = x.NameEN,
                NameKO = x.NameKO,
                Order = x.Order ?? 0,
                View = true,
                Add = true,
                Edit = true,
                Delete = true,
                Approve = true,
                IsParent = x.IsParent ?? false,
                CodeParent = x.CodeParent,
                Icon = x.Icon,
                Url = x.Url
            });
        }

        // Get permissions from MenuRoles
        var menuRolePermissions = await _context.MenuRoles
            .Where(mr => mr.UserRoleId != null && roleIds.Contains(mr.UserRoleId.Value))
            .ToListAsync();

        // Get permissions from UserMenus (direct)
        var userMenuPermissions = await _context.UserMenus
            .Where(um => um.UserId == userId)
            .ToListAsync();

        // DEBUG LOG EXTENDED
        try {
            var logPath = Path.Combine(Directory.GetCurrentDirectory(), "permission_debug.log");
            File.AppendAllText(logPath, $" - RolePerms: {menuRolePermissions.Count}, UserPerms: {userMenuPermissions.Count}\n");
        } catch {}

        var result = new List<MenuPermissionDto>();

        foreach (var menu in allMenus)
        {
            var rolePerms = menuRolePermissions.Where(rp => rp.MenuId == menu.Id || (!string.IsNullOrEmpty(rp.MenuCode) && rp.MenuCode == menu.Code)).ToList();
            var directPerms = userMenuPermissions.Where(up => up.MenuId == menu.Id || (!string.IsNullOrEmpty(up.MenuCode) && up.MenuCode == menu.Code)).ToList();

            bool canView = rolePerms.Any(p => p.View == true) || directPerms.Any(p => p.View == true);
            
            // Safety: System menus only for SuperAdmin
            var systemOnly = new HashSet<string> { "system", "system/roles", "system/security", "menus", "system/menus" };
            if (!isSuperAdmin && systemOnly.Contains(menu.Code?.ToLower() ?? ""))
            {
                canView = false;
            }

            if (canView)
            {
                result.Add(new MenuPermissionDto
                {
                    Id = menu.Id,
                    MenuCode = menu.Code,
                    Name = menu.Name,
                    NameEN = menu.NameEN,
                    NameKO = menu.NameKO,
                    Order = menu.Order ?? 0,
                    View = true,
                    Add = rolePerms.Any(p => p.Add == true) || directPerms.Any(p => p.Add == true),
                    Edit = rolePerms.Any(p => p.Edit == true) || directPerms.Any(p => p.Edit == true),
                    Delete = rolePerms.Any(p => p.Delete == true) || directPerms.Any(p => p.Delete == true),
                    Approve = rolePerms.Any(p => p.Approve == true) || directPerms.Any(p => p.Approve == true),
                    IsParent = menu.IsParent ?? false,
                    CodeParent = menu.CodeParent,
                    Icon = menu.Icon,
                    Url = menu.Url
                });
            }
        }

        return result;
    }

    public async Task<MenuPermissionDto?> CheckRole(string menuCode, List<string> roleCodes)
    {
        var menu = await _context.Menus.FirstOrDefaultAsync(x => x.Code == menuCode);
        if (menu == null) return null;

        if (roleCodes.Contains("SuperAdmin"))
        {
            return new MenuPermissionDto
            {
                Id = menu.Id,
                MenuCode = menu.Code,
                Name = menu.Name,
                View = true, Add = true, Edit = true, Delete = true, Approve = true
            };
        }

        var roles = await _context.UserRoles.Where(r => roleCodes.Contains(r.Code)).Select(r => r.Id).ToListAsync();
        
        var rolePerms = await _context.MenuRoles
            .Where(mr => (mr.MenuId == menu.Id || mr.MenuCode == menu.Code) && mr.UserRoleId != null && roles.Contains(mr.UserRoleId.Value))
            .ToListAsync();

        if (!rolePerms.Any()) return null;

        return new MenuPermissionDto
        {
            Id = menu.Id,
            MenuCode = menu.Code,
            Name = menu.Name,
            View = rolePerms.Any(p => p.View == true),
            Add = rolePerms.Any(p => p.Add == true),
            Edit = rolePerms.Any(p => p.Edit == true),
            Delete = rolePerms.Any(p => p.Delete == true),
            Approve = rolePerms.Any(p => p.Approve == true)
        };
    }

    public async Task<Menu?> GetById(int id)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task<Menu?> GetById(int id, List<string> roles, int userId)
    {
        return await _context.Menus.FindAsync(id);
    }

    public async Task Create(Menu menu)
    {
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
    }

    public async Task Create(MenuViewModel model)
    {
        var existCode = await _context.Menus.FirstOrDefaultAsync(x => x.Code == model.Code);
        if (existCode != null)
        {
            throw new ArgumentException("Mã CODE này đã tồn tại trong hệ thống. Vui lòng nhập mã khác.");
        }

        var menu = new Menu
        {
            Code = model.Code,
            Name = model.Name,
            NameEN = model.NameEN,
            NameKO = model.NameKO,
            CodeParent = string.IsNullOrEmpty(model.CodeParent) ? null : model.CodeParent,
            IsParent = model.IsParent,
            Order = model.Order,
            Note = model.Note,
            Icon = model.Icon
        };
        _context.Menus.Add(menu);
        await _context.SaveChangesAsync();
    }

    public async Task Update(Menu menu)
    {
        _context.Menus.Update(menu);
        await _context.SaveChangesAsync();
    }

    public async Task Update(MenuViewModel model, List<string> roles, int userId)
    {
        var existCode = await _context.Menus.FirstOrDefaultAsync(x => x.Code == model.Code && x.Id != model.Id);
        if (existCode != null)
        {
            throw new ArgumentException("Mã CODE này đã tồn tại trong hệ thống. Vui lòng nhập mã khác.");
        }

        var menu = await _context.Menus.FindAsync(model.Id);
        if (menu != null)
        {
            menu.Code = model.Code;
            menu.Name = model.Name;
            menu.NameEN = model.NameEN;
            menu.NameKO = model.NameKO;
            menu.CodeParent = string.IsNullOrEmpty(model.CodeParent) ? null : model.CodeParent;
            menu.IsParent = model.IsParent;
            menu.Order = model.Order;
            menu.Note = model.Note;
            menu.Icon = model.Icon;
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
        }
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
}
