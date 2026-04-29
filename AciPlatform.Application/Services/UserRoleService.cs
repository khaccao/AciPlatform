using AciPlatform.Application.Interfaces;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IApplicationDbContext _context;

    public UserRoleService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserRole>> GetAll()
    {
        return await _context.UserRoles.OrderBy(x => x.Order).ToListAsync();
    }

    public async Task<IEnumerable<UserRole>> GetAll(int userId, List<string> roles)
    {
        // Return all roles (can add role filtering logic if needed)
        return await _context.UserRoles.OrderBy(x => x.Order).ToListAsync();
    }

    public async Task<UserRole?> GetById(int id)
    {
        return await _context.UserRoles.FindAsync(id);
    }

    public async Task<UserRole> Create(UserRole role)
    {
        if (await _context.UserRoles.AnyAsync(x => x.Code == role.Code))
            throw new Exception($"Role code {role.Code} already exists");

        _context.UserRoles.Add(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task<UserRole> Update(UserRole role)
    {
        _context.UserRoles.Update(role);
        await _context.SaveChangesAsync();
        return role;
    }

    public async Task Delete(int id)
    {
        var role = await _context.UserRoles.FindAsync(id);
        if (role != null)
        {
            if (role.IsNotAllowDelete == true)
                throw new Exception("This role is not allowed to be deleted");

            _context.UserRoles.Remove(role);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<UserRole>> GetAll(string? companyCode)
    {
        var query = _context.UserRoles.AsQueryable();

        if (string.IsNullOrEmpty(companyCode))
        {
            // If no company code (SuperAdmin or system), maybe return all? 
            // Or typically SuperAdmin sees all.
        }
        else
        {
            // Filter by CompanyCode
            // Assuming NULL CompanyCode means "System" or "Global" roles that everyone can see (like 'User')
            // Or maybe restrict strictly. Let's assume strict + shared system roles.
            query = query.Where(x => x.CompanyCode == companyCode || x.CompanyCode == null);
        }

        return await query.OrderBy(x => x.Order).ToListAsync();
    }
}

