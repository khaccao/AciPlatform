using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.Hotel;
using AciPlatform.Domain.Entities;
using AciPlatform.Domain.Entities.HoSoNhanSu;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Application.Services.Hotel;

/// <summary>
/// Hotel Service — tận dụng lại bảng Customers và UserCompanies.
/// Nguyên tắc: CompanyCode = HotelCode, IsHotel=true.
/// Khi gán user vào Hotel = gán user vào Company trong UserCompanies.
/// </summary>
public class HotelService : IHotelService
{
    private readonly IApplicationDbContext _context;

    public HotelService(IApplicationDbContext context)
    {
        _context = context;
    }

    // ── Hotels (Company where IsHotel=true) ────────────────────

    public async Task<List<HotelDto>> GetAllHotelsAsync()
    {
        return await _context.Customers
            .Where(c => c.IsHotel && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Select(c => ToDto(c))
            .ToListAsync();
    }

    public async Task<HotelDto?> GetHotelByCodeAsync(string code)
    {
        var c = await _context.Customers
            .FirstOrDefaultAsync(x => x.Code == code && x.IsHotel && !x.IsDeleted);
        return c == null ? null : ToDto(c);
    }

    public async Task<List<HotelDto>> GetHotelsByUserAsync(int userId)
    {
        var codes = await _context.UserCompanies
            .Where(uc => uc.UserId == userId)
            .Select(uc => uc.CompanyCode)
            .ToListAsync();

        return await _context.Customers
            .Where(c => codes.Contains(c.Code!) && c.IsHotel && !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Select(c => ToDto(c))
            .ToListAsync();
    }

    public async Task<HotelDto> UpsertHotelAsync(UpsertHotelRequest request)
    {
        var existing = await _context.Customers
            .FirstOrDefaultAsync(c => c.Code == request.Code);

        if (existing == null)
        {
            existing = new Customer
            {
                Code        = request.Code,
                CreatedDate = DateTime.Now,
            };
            _context.Customers.Add(existing);
        }

        existing.Name               = request.Name;
        existing.Phone              = request.Phone;
        existing.Email              = request.Email;
        existing.Address            = request.Address;
        existing.IsHotel            = true;
        existing.HotelType          = request.HotelType;
        existing.PmsConnectionString = request.PmsConnectionString;
        existing.DmsAppId           = request.DmsAppId;
        existing.DmsAppSecret       = request.DmsAppSecret;
        existing.UpdatedDate        = DateTime.Now;
        existing.IsDeleted          = false;

        await _context.SaveChangesAsync(default);
        return ToDto(existing);
    }

    // ── User ↔ Hotel Assignment ────────────────────────────────

    public async Task AssignUserToHotelAsync(AssignHotelUserRequest request)
    {
        // Validate hotel exists
        var hotel = await _context.Customers
            .FirstOrDefaultAsync(c => c.Code == request.HotelCode && c.IsHotel && !c.IsDeleted)
            ?? throw new InvalidOperationException($"Hotel '{request.HotelCode}' not found.");

        var existing = await _context.UserCompanies
            .FirstOrDefaultAsync(uc => uc.UserId == request.UserId && uc.CompanyCode == request.HotelCode);

        if (existing == null)
        {
            _context.UserCompanies.Add(new UserCompany
            {
                UserId      = request.UserId,
                CompanyCode = request.HotelCode,
                UserFO      = request.UserFO,
                UserBO      = request.UserBO,
                UserPOS     = request.UserPOS,
            });
        }
        else
        {
            // Only update PMS fields if provided — keep existing if null
            if (request.UserFO  != null) existing.UserFO  = request.UserFO;
            if (request.UserBO  != null) existing.UserBO  = request.UserBO;
            if (request.UserPOS != null) existing.UserPOS = request.UserPOS;
        }

        await _context.SaveChangesAsync(default);
    }

    public async Task RemoveUserFromHotelAsync(int userId, string hotelCode)
    {
        var record = await _context.UserCompanies
            .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CompanyCode == hotelCode);
        if (record != null)
        {
            _context.UserCompanies.Remove(record);
            await _context.SaveChangesAsync(default);
        }
    }

    public async Task<List<HotelUserDto>> GetUsersOfHotelAsync(string hotelCode)
    {
        return await _context.UserCompanies
            .Where(uc => uc.CompanyCode == hotelCode)
            .Join(_context.Users,
                uc => uc.UserId,
                u  => u.Id,
                (uc, u) => new HotelUserDto
                {
                    UserId   = u.Id,
                    FullName = u.FullName,
                    Username = u.Username,
                    Email    = u.Email,
                    Avatar   = u.Avatar,
                    UserFO   = uc.UserFO,
                    UserBO   = uc.UserBO,
                    UserPOS  = uc.UserPOS,
                })
            .ToListAsync();
    }

    public async Task<string?> GetUserFOAsync(int userId, string hotelCode)
    {
        var uc = await _context.UserCompanies
            .FirstOrDefaultAsync(x => x.UserId == userId && x.CompanyCode == hotelCode);
        return uc?.UserFO;
    }

    // ── Mapper ────────────────────────────────────────────────
    private static HotelDto ToDto(Customer c) => new()
    {
        Id                  = c.Id,
        Code                = c.Code,
        Name                = c.Name,
        Phone               = c.Phone,
        Email               = c.Email,
        Address             = c.Address,
        Avatar              = c.Avatar,
        HotelType           = c.HotelType,
        IsActive            = !c.IsDeleted,
        PmsConnectionString = c.PmsConnectionString,
        DmsAppId            = c.DmsAppId,
        DmsAppSecret        = c.DmsAppSecret,
    };
}
