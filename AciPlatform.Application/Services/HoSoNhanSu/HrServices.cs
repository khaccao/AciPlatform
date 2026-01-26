using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.HoSoNhanSu;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.HoSoNhanSu;

public class DepartmentService : IDepartmentService
{
    private readonly IApplicationDbContext _context;

    public DepartmentService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Department>> GetAllAsync()
    {
        return await _context.Departments
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Order ?? x.Id)
            .ToListAsync();
    }

    public async Task<Department?> GetByIdAsync(int id)
    {
        return await _context.Departments.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Department> CreateAsync(DepartmentRequest request)
    {
        var entity = new Department
        {
            Name = request.Name.Trim(),
            Code = request.Code?.Trim(),
            ParentId = request.ParentId,
            Order = request.Order,
            CreatedDate = DateTime.UtcNow
        };
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, DepartmentRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Department not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code?.Trim();
        entity.ParentId = request.ParentId;
        entity.Order = request.Order;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Departments.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Departments.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class PositionDetailService : IPositionDetailService
{
    private readonly IApplicationDbContext _context;

    public PositionDetailService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PositionDetail>> GetAllAsync()
    {
        return await _context.PositionDetails
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Order ?? x.Id)
            .ToListAsync();
    }

    public async Task<PositionDetail?> GetByIdAsync(int id)
    {
        return await _context.PositionDetails.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<PositionDetail> CreateAsync(PositionDetailRequest request)
    {
        var entity = new PositionDetail
        {
            Name = request.Name.Trim(),
            Code = request.Code?.Trim(),
            DepartmentId = request.DepartmentId,
            Note = request.Note,
            Order = request.Order,
            CreatedDate = DateTime.UtcNow
        };
        _context.PositionDetails.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, PositionDetailRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Position detail not found");
        entity.Name = request.Name.Trim();
        entity.Code = request.Code?.Trim();
        entity.DepartmentId = request.DepartmentId;
        entity.Note = request.Note;
        entity.Order = request.Order;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.PositionDetails.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.PositionDetails.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class DegreeService : IDegreeService
{
    private readonly IApplicationDbContext _context;

    public DegreeService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Degree>> GetAllAsync()
    {
        return await _context.Degrees.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<Degree?> GetByIdAsync(int id)
    {
        return await _context.Degrees.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Degree> CreateAsync(DegreeRequest request)
    {
        var entity = new Degree
        {
            Name = request.Name.Trim(),
            School = request.School,
            Description = request.Description,
            GraduationYear = request.GraduationYear,
            CreatedDate = DateTime.UtcNow
        };
        _context.Degrees.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, DegreeRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Degree not found");
        entity.Name = request.Name.Trim();
        entity.School = request.School;
        entity.Description = request.Description;
        entity.GraduationYear = request.GraduationYear;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Degrees.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Degrees.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class CertificateService : ICertificateService
{
    private readonly IApplicationDbContext _context;

    public CertificateService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Certificate>> GetAllAsync()
    {
        return await _context.Certificates.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<Certificate?> GetByIdAsync(int id)
    {
        return await _context.Certificates.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Certificate> CreateAsync(CertificateRequest request)
    {
        var entity = new Certificate
        {
            Name = request.Name.Trim(),
            Issuer = request.Issuer,
            IssueDate = request.IssueDate,
            ExpiryDate = request.ExpiryDate,
            Note = request.Note,
            CreatedDate = DateTime.UtcNow
        };
        _context.Certificates.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, CertificateRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Certificate not found");
        entity.Name = request.Name.Trim();
        entity.Issuer = request.Issuer;
        entity.IssueDate = request.IssueDate;
        entity.ExpiryDate = request.ExpiryDate;
        entity.Note = request.Note;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Certificates.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Certificates.Update(entity);
        await _context.SaveChangesAsync();
    }
}

public class MajorService : IMajorService
{
    private readonly IApplicationDbContext _context;

    public MajorService(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Major>> GetAllAsync()
    {
        return await _context.Majors.Where(x => !x.IsDeleted).OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<Major?> GetByIdAsync(int id)
    {
        return await _context.Majors.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Major> CreateAsync(MajorRequest request)
    {
        var entity = new Major
        {
            Name = request.Name.Trim(),
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };
        _context.Majors.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(int id, MajorRequest request)
    {
        var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException("Major not found");
        entity.Name = request.Name.Trim();
        entity.Description = request.Description;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Majors.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return;
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _context.Majors.Update(entity);
        await _context.SaveChangesAsync();
    }
}
