using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.ChamCong;
using AciPlatform.Domain.Entities.ChamCong;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.ChamCong;

public class TimeKeepingService : ITimeKeepingService
{
    private readonly IApplicationDbContext _context;

    public TimeKeepingService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TimeKeepingEntry>> GetByRangeAsync(int? userId, DateTime? from, DateTime? to)
    {
        var query = _context.TimeKeepingEntries.Where(x => !x.IsDeleted);
        if (userId.HasValue && userId.Value > 0)
        {
            query = query.Where(x => x.UserId == userId.Value);
        }
        if (from.HasValue)
        {
            query = query.Where(x => x.WorkDate >= from.Value.Date);
        }
        if (to.HasValue)
        {
            query = query.Where(x => x.WorkDate <= to.Value.Date);
        }
        return await query.OrderByDescending(x => x.WorkDate).ToListAsync();
    }

    public async Task<TimeKeepingEntry?> GetByIdAsync(int id)
    {
        return await _context.TimeKeepingEntries.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<TimeKeepingEntry> CreateAsync(TimeKeepingEntryRequest request)
    {
        var entity = new TimeKeepingEntry
        {
            UserId = request.UserId,
            WorkDate = request.WorkDate.Date,
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            WorkingHours = request.WorkingHours,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        if (entity.WorkingHours == null && entity.CheckIn.HasValue && entity.CheckOut.HasValue)
        {
            entity.WorkingHours = (entity.CheckOut.Value - entity.CheckIn.Value).TotalHours;
        }
        _context.TimeKeepingEntries.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, TimeKeepingEntryRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Time keeping entry not found");
        entity.UserId = request.UserId;
        entity.WorkDate = request.WorkDate.Date;
        entity.CheckIn = request.CheckIn;
        entity.CheckOut = request.CheckOut;
        entity.WorkingHours = request.WorkingHours;
        entity.Note = request.Note;
        if (entity.WorkingHours == null && entity.CheckIn.HasValue && entity.CheckOut.HasValue)
        {
            entity.WorkingHours = (entity.CheckOut.Value - entity.CheckIn.Value).TotalHours;
        }
        entity.UpdatedDate = DateTime.UtcNow;
        _context.TimeKeepingEntries.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.TimeKeepingEntries.Update(entity);
        await _context.SaveChangesAsync();
    }
}
