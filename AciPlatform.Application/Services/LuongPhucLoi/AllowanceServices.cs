using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.LuongPhucLoi;
using AciPlatform.Domain.Entities.LuongPhucLoi;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.LuongPhucLoi;

public class AllowanceService : IAllowanceService
{
    private readonly IApplicationDbContext _context;

    public AllowanceService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Allowance>> GetAllAsync()
    {
        return await _context.Allowances.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<Allowance?> GetByIdAsync(int id)
    {
        return await _context.Allowances.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Allowance> CreateAsync(AllowanceRequest request)
    {
        var entity = new Allowance
        {
            Name = request.Name.Trim(),
            Code = request.Code,
            Amount = request.Amount,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };
        _context.Allowances.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, AllowanceRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Allowance not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code;
        entity.Amount = request.Amount;
        entity.Description = request.Description;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Allowances.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Allowances.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class AllowanceUserService : IAllowanceUserService
{
    private readonly IApplicationDbContext _context;

    public AllowanceUserService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AllowanceUser>> GetByUserAsync(int userId)
    {
        return await _context.AllowanceUsers
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.StartDate ?? x.CreatedDate)
            .ToListAsync();
    }

    public async Task<AllowanceUser?> GetByIdAsync(int id)
    {
        return await _context.AllowanceUsers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<AllowanceUser> CreateAsync(AllowanceUserRequest request)
    {
        var entity = new AllowanceUser
        {
            AllowanceId = request.AllowanceId,
            UserId = request.UserId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AmountOverride = request.AmountOverride,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.AllowanceUsers.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, AllowanceUserRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Allowance assignment not found");
        entity.AllowanceId = request.AllowanceId;
        entity.UserId = request.UserId;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.AmountOverride = request.AmountOverride;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.AllowanceUsers.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.AllowanceUsers.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class SalaryTypeService : ISalaryTypeService
{
    private readonly IApplicationDbContext _context;

    public SalaryTypeService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<SalaryType>> GetAllAsync()
    {
        return await _context.SalaryTypes.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<SalaryType?> GetByIdAsync(int id)
    {
        return await _context.SalaryTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<SalaryType> CreateAsync(SalaryTypeRequest request)
    {
        var entity = new SalaryType
        {
            Name = request.Name.Trim(),
            Code = request.Code,
            BaseAmount = request.BaseAmount,
            Formula = request.Formula,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.SalaryTypes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, SalaryTypeRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Salary type not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code;
        entity.BaseAmount = request.BaseAmount;
        entity.Formula = request.Formula;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.SalaryTypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.SalaryTypes.Update(entity);
        await _context.SaveChangesAsync();
    }
}
