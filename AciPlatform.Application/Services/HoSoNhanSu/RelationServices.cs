using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.HoSoNhanSu;

public class RelativeService : IRelativeService
{
    private readonly IApplicationDbContext _context;

    public RelativeService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Relative>> GetByUserAsync(int userId)
    {
        return await _context.Relatives
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.Id)
            .ToListAsync();
    }

    public async Task<Relative?> GetByIdAsync(int id)
    {
        return await _context.Relatives.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Relative> CreateAsync(RelativeRequest request)
    {
        var entity = new Relative
        {
            UserId = request.UserId,
            Name = request.Name.Trim(),
            Relationship = request.Relationship.Trim(),
            Phone = request.Phone,
            Address = request.Address,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.Relatives.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, RelativeRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Relative not found");
        entity.UserId = request.UserId;
        entity.Name = request.Name.Trim();
        entity.Relationship = request.Relationship.Trim();
        entity.Phone = request.Phone;
        entity.Address = request.Address;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Relatives.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Relatives.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class HistoryAchievementService : IHistoryAchievementService
{
    private readonly IApplicationDbContext _context;

    public HistoryAchievementService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HistoryAchievement>> GetByUserAsync(int userId)
    {
        return await _context.HistoryAchievements
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.AchievedDate ?? x.CreatedDate)
            .ToListAsync();
    }

    public async Task<HistoryAchievement?> GetByIdAsync(int id)
    {
        return await _context.HistoryAchievements.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<HistoryAchievement> CreateAsync(HistoryAchievementRequest request)
    {
        var entity = new HistoryAchievement
        {
            UserId = request.UserId,
            Title = request.Title.Trim(),
            Description = request.Description,
            AchievedDate = request.AchievedDate,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.HistoryAchievements.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, HistoryAchievementRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("History achievement not found");
        entity.UserId = request.UserId;
        entity.Title = request.Title.Trim();
        entity.Description = request.Description;
        entity.AchievedDate = request.AchievedDate;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.HistoryAchievements.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.HistoryAchievements.Update(entity);
        await _context.SaveChangesAsync();
    }
}
