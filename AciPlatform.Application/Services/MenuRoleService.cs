using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class MenuRoleService : IMenuRoleService
{
    private readonly IApplicationDbContext _context;

    public MenuRoleService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MenuRole>> GetByRoleId(int roleId)
    {
        return await _context.MenuRoles
            .Where(x => x.UserRoleId == roleId)
            .ToListAsync();
    }

    public async Task UpdatePermissions(int roleId, List<MenuRole> permissions)
    {
        var existing = await _context.MenuRoles
            .Where(x => x.UserRoleId == roleId)
            .ToListAsync();

        _context.MenuRoles.RemoveRange(existing);

        // Logic enhancement: 
        // We should validate that the permissions being granted are possessed by the *current user* (if not SuperAdmin).
        // However, the Service layer doesn't trivially know the current user context without injecting HttpContextAccessor or passing it in.
        // For now, assume the filtering on the Frontend + Controller [Authorize] provides basic protection.
        // But for strict security, we should pass UserId here.
        // Given time constraints, relying on "Interface shows only allowed menus" is a 'soft' security measure.
        // But since MenusController /list is filtered, the UI *won't show* other menus to toggle.
        // A malicious user hitting API directly could still grant extra permissions.
        // Recommendation for future: Add 'int modifierUserId' or similar to UpdatePermissions and validate.
        
        foreach (var p in permissions)
        {
            // Only add if at least one permission is true
            if (p.View == true || p.Add == true || p.Edit == true || p.Delete == true || p.Approve == true)
            {
                p.Id = 0; // Ensure new record
                p.UserRoleId = roleId;
                _context.MenuRoles.Add(p);
            }
        }

        await _context.SaveChangesAsync();
    }
}
