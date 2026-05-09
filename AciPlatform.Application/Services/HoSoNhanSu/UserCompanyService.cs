using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using Microsoft.EntityFrameworkCore;
using AciPlatform.Application.Interfaces;

namespace AciPlatform.Application.Services.HoSoNhanSu;

public class UserCompanyService : IUserCompanyService
{
    private readonly IApplicationDbContext _context;

    public UserCompanyService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<string>> GetCompanyCodesByUsername(string username)
    {
        if (string.IsNullOrEmpty(username)) return new List<string>();
        
        var user = await _context.Users
            .Where(u => u.Username == username && !u.IsDeleted)
            .FirstOrDefaultAsync();

        if (user == null) return new List<string>();

        return await _context.UserCompanies
            .Where(uc => uc.UserId == user.Id)
            .Select(uc => uc.CompanyCode)
            .Distinct()
            .ToListAsync();
    }

    public async Task<UserCompany> CreateAsync(int userId, string companyCode)
    {
        var entity = new UserCompany { UserId = userId, CompanyCode = companyCode.Trim() };
        _context.UserCompanies.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> ExistsAsync(int userId, string companyCode)
    {
        return await _context.UserCompanies.AnyAsync(x => x.UserId == userId && x.CompanyCode == companyCode);
    }

    public async Task ClearAsync(int userId)
    {
        var entities = await _context.UserCompanies.Where(x => x.UserId == userId).ToListAsync();
        _context.UserCompanies.RemoveRange(entities);
        await _context.SaveChangesAsync();
    }
}

