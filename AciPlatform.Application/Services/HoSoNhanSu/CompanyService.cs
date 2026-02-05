using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.HoSoNhanSu;

public class CompanyService : ICompanyService
{
    private readonly IApplicationDbContext _context;

    public CompanyService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync()
    {
        return await _context.Customers
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Customer?> GetByCodeAsync(string code)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Code == code && !c.IsDeleted);
    }
}
