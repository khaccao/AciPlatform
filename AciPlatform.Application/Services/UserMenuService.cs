using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class UserMenuService : IUserMenuService
{
    private readonly IApplicationDbContext _context;

    public UserMenuService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserMenu>> GetByUserId(int userId)
    {
        return await _context.UserMenus
            .Where(um => um.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<MenuPermissionDto>> GetMenuPermissionsForUser(int userId)
    {
        var userMenus = await _context.UserMenus
            .Where(um => um.UserId == userId)
            .Include(um => um.Menu)
            .ToListAsync();

        return userMenus.Select(um => new MenuPermissionDto
        {
            Id = um.MenuId,
            MenuCode = um.MenuCode ?? um.Menu?.Code ?? string.Empty,
            Name = um.Menu?.Name,
            NameEN = um.Menu?.NameEN,
            NameKO = um.Menu?.NameKO,
            Order = um.Menu?.Order,
            Icon = um.Menu?.Icon,
            View = um.View,
            Add = um.Add,
            Edit = um.Edit,
            Delete = um.Delete
        }).OrderBy(m => m.Order).ToList();
    }

    public async Task AssignMenusToUser(int userId, List<UserMenuAssignDto> menus, int createdBy)
    {
        // Validate user exists
        var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
        if (!userExists)
            throw new Exception($"User with ID {userId} not found");

        // Validate all menuIds exist
        var menuIds = menus.Select(m => m.MenuId).Distinct().ToList();
        var existingMenuIds = await _context.Menus
            .Where(m => menuIds.Contains(m.Id))
            .Select(m => m.Id)
            .ToListAsync();
        
        var invalidMenuIds = menuIds.Except(existingMenuIds).ToList();
        if (invalidMenuIds.Any())
            throw new Exception($"Menu IDs not found: {string.Join(", ", invalidMenuIds)}");

        // Remove existing user menus
        var existingMenus = await _context.UserMenus
            .Where(um => um.UserId == userId)
            .ToListAsync();
        
        _context.UserMenus.RemoveRange(existingMenus);

        // Add new user menus
        var newUserMenus = menus.Select(m => new UserMenu
        {
            UserId = userId,
            MenuId = m.MenuId,
            MenuCode = m.MenuCode,
            View = m.View,
            Add = m.Add,
            Edit = m.Edit,
            Delete = m.Delete,
            CreatedDate = DateTime.UtcNow,
            CreatedBy = createdBy
        }).ToList();

        _context.UserMenus.AddRange(newUserMenus);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveMenuFromUser(int userId, int menuId)
    {
        var userMenu = await _context.UserMenus
            .FirstOrDefaultAsync(um => um.UserId == userId && um.MenuId == menuId);
        
        if (userMenu != null)
        {
            _context.UserMenus.Remove(userMenu);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearUserMenus(int userId)
    {
        var userMenus = await _context.UserMenus
            .Where(um => um.UserId == userId)
            .ToListAsync();
        
        _context.UserMenus.RemoveRange(userMenus);
        await _context.SaveChangesAsync();
    }
}

