using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.ChamCong;

namespace AciPlatform.Application.Interfaces.ChamCong;

public interface ITimeKeepingService
{
    Task<IEnumerable<TimeKeepingEntry>> GetByRangeAsync(int? userId, DateTime? from, DateTime? to);
    Task<TimeKeepingEntry?> GetByIdAsync(int id);
    Task<TimeKeepingEntry> CreateAsync(TimeKeepingEntryRequest request);
    Task UpdateAsync(int id, TimeKeepingEntryRequest request);
    Task DeleteAsync(int id);
}
