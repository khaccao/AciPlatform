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
        return await (from uc in _context.UserCompanies
                      join u in _context.Users on uc.UserId equals u.Id
                      where u.Username == username && !u.IsDeleted
                      select uc.CompanyCode).Distinct().ToListAsync();
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
