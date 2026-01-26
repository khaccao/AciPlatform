using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HopDong;
using AciPlatform.Domain.Entities.HopDong;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.HopDong;

public class ContractTypeService : IContractTypeService
{
    private readonly IApplicationDbContext _context;

    public ContractTypeService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractType>> GetAllAsync()
    {
        return await _context.ContractTypes.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<ContractType?> GetByIdAsync(int id)
    {
        return await _context.ContractTypes.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<ContractType> CreateAsync(ContractTypeRequest request)
    {
        var entity = new ContractType
        {
            Name = request.Name.Trim(),
            Code = request.Code,
            Description = request.Description,
            DurationMonths = request.DurationMonths,
            CreatedDate = DateTime.UtcNow
        };
        _context.ContractTypes.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, ContractTypeRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Contract type not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code;
        entity.Description = request.Description;
        entity.DurationMonths = request.DurationMonths;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.ContractTypes.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.ContractTypes.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class ContractFileService : IContractFileService
{
    private readonly IApplicationDbContext _context;

    public ContractFileService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ContractFile>> GetAllAsync()
    {
        return await _context.ContractFiles.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<ContractFile?> GetByIdAsync(int id)
    {
        return await _context.ContractFiles.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<ContractFile> CreateAsync(ContractFileRequest request)
    {
        var entity = new ContractFile
        {
            Name = request.Name.Trim(),
            FileUrl = request.FileUrl,
            ContractTypeId = request.ContractTypeId,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.ContractFiles.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, ContractFileRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Contract file not found");
        entity.Name = request.Name.Trim();
        entity.FileUrl = request.FileUrl;
        entity.ContractTypeId = request.ContractTypeId;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.ContractFiles.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.ContractFiles.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class UserContractHistoryService : IUserContractHistoryService
{
    private readonly IApplicationDbContext _context;

    public UserContractHistoryService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserContractHistory>> GetByUserAsync(int userId)
    {
        return await _context.UserContractHistories
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.StartDate ?? x.CreatedDate)
            .ToListAsync();
    }

    public async Task<UserContractHistory?> GetByIdAsync(int id)
    {
        return await _context.UserContractHistories.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<UserContractHistory> CreateAsync(UserContractHistoryRequest request)
    {
        var entity = new UserContractHistory
        {
            UserId = request.UserId,
            ContractTypeId = request.ContractTypeId,
            SignedDate = request.SignedDate,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            FileUrl = request.FileUrl,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.UserContractHistories.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, UserContractHistoryRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("User contract history not found");
        entity.UserId = request.UserId;
        entity.ContractTypeId = request.ContractTypeId;
        entity.SignedDate = request.SignedDate;
        entity.StartDate = request.StartDate;
        entity.EndDate = request.EndDate;
        entity.FileUrl = request.FileUrl;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.UserContractHistories.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.UserContractHistories.Update(entity);
        await _context.SaveChangesAsync();
    }
}
