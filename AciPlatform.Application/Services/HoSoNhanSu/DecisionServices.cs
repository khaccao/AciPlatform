using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.HoSoNhanSu;

public class DecisionTypeService : IDecisionTypeService
{
    private readonly IApplicationDbContext _context;

    public DecisionTypeService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DecisionType>> GetAllAsync()
    {
        return await _context.DecisionTypes.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<DecisionType?> GetByIdAsync(int id)
    {
        return await _context.DecisionTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<DecisionType> CreateAsync(DecisionTypeRequest request)
    {
        var entity = new DecisionType
        {
            Name = request.Name.Trim(),
            Code = request.Code,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.DecisionTypes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, DecisionTypeRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Decision type not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.DecisionTypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.DecisionTypes.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class DecideService : IDecideService
{
    private readonly IApplicationDbContext _context;

    public DecideService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Decide>> GetAllAsync()
    {
        return await _context.Decides
            .Where(x => !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<IEnumerable<Decide>> GetByUserAsync(int userId)
    {
        return await _context.Decides
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.EffectiveDate ?? x.CreatedDate)
            .ToListAsync();
    }

    public async Task<Decide?> GetByIdAsync(int id)
    {
        return await _context.Decides.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Decide> CreateAsync(DecideRequest request)
    {
        var entity = new Decide
        {
            UserId = request.UserId,
            DecisionTypeId = request.DecisionTypeId,
            Title = request.Title.Trim(),
            Description = request.Description,
            EffectiveDate = request.EffectiveDate,
            ExpiredDate = request.ExpiredDate,
            FileUrl = request.FileUrl,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.Decides.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, DecideRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Decision record not found");
        entity.UserId = request.UserId;
        entity.DecisionTypeId = request.DecisionTypeId;
        entity.Title = request.Title.Trim();
        entity.Description = request.Description;
        entity.EffectiveDate = request.EffectiveDate;
        entity.ExpiredDate = request.ExpiredDate;
        entity.FileUrl = request.FileUrl;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Decides.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Decides.Update(entity);
        await _context.SaveChangesAsync();
    }
}
