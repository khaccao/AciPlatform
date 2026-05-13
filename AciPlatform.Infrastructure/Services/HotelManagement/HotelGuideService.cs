using AciPlatform.Application.Interfaces.HotelManagement;
using AciPlatform.Domain.Entities.Hotel;
using AciPlatform.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AciPlatform.Infrastructure.Services.HotelManagement;

public class HotelGuideService : IHotelGuideService
{
    private readonly HotelDbContext _db;
    public HotelGuideService(HotelDbContext db) => _db = db;

    // ── CRUD Guides ────────────────────────────────────────────
    public async Task<List<HotelTourGuideDto>> GetGuidesAsync(string hotelCode, bool? isActive = null)
    {
        var q = _db.HotelTourGuides.Where(g => g.HotelCode == hotelCode);
        if (isActive.HasValue) q = q.Where(g => g.IsActive == isActive.Value);
        var guides = await q.OrderBy(g => g.Name).ToListAsync();

        // Enrich with salary & schedule stats
        var currentYear = DateTime.Now.Year;
        var guideIds = guides.Select(g => g.Id).ToList();

        var tourCounts = await _db.HotelTourSchedules
            .Where(s => s.HotelCode == hotelCode && guideIds.Contains(s.GuideId ?? 0) && s.TourDate.Year == currentYear)
            .GroupBy(s => s.GuideId)
            .Select(g => new { GuideId = g.Key, Count = g.Count() })
            .ToListAsync();

        var contracts = await _db.PmsTourGuideContracts
            .Where(c => c.HotelCode == hotelCode && guideIds.Contains(c.GuideId))
            .OrderByDescending(c => c.StartDate)
            .ToListAsync();

        return guides.Select(g =>
        {
            var latestContract = contracts.FirstOrDefault(c => c.GuideId == g.Id);
            var count = tourCounts.FirstOrDefault(tc => tc.GuideId == g.Id)?.Count ?? 0;
            return ToDto(g, latestContract, count);
        }).ToList();
    }

    public async Task<HotelTourGuideDto?> GetGuideByIdAsync(int id)
    {
        var g = await _db.HotelTourGuides.FindAsync(id);
        if (g == null) return null;
        var contract = await _db.PmsTourGuideContracts.Where(c => c.GuideId == id).OrderByDescending(c => c.StartDate).FirstOrDefaultAsync();
        var count = await _db.HotelTourSchedules.CountAsync(s => s.GuideId == id && s.TourDate.Year == DateTime.Now.Year);
        return ToDto(g, contract, count);
    }

    public async Task<HotelTourGuideDto> UpsertGuideAsync(UpsertTourGuideRequest req)
    {
        HotelTourGuide? g;
        if (!string.IsNullOrEmpty(req.GuideCode))
            g = await _db.HotelTourGuides.FirstOrDefaultAsync(x => x.HotelCode == req.HotelCode && x.GuideCode == req.GuideCode);
        else
            g = null;

        if (g == null)
        {
            g = new HotelTourGuide { HotelCode = req.HotelCode };
            // Auto-generate guide code if not provided
            if (string.IsNullOrEmpty(req.GuideCode))
            {
                var count = await _db.HotelTourGuides.CountAsync(x => x.HotelCode == req.HotelCode);
                g.GuideCode = $"HDV{count + 1:D3}";
            }
            else g.GuideCode = req.GuideCode;
            _db.HotelTourGuides.Add(g);
        }

        g.Name = req.Name; g.Phone = req.Phone; g.Email = req.Email;
        g.Languages = req.Languages; g.Speciality = req.Speciality;
        g.IsFreelance = req.IsFreelance; g.DailyRate = req.DailyRate;
        g.Bio = req.Bio; g.IsActive = req.IsActive;
        g.HrEmployeeId = req.HrEmployeeId; g.IdCard = req.IdCard;
        g.Address = req.Address; g.BirthDate = req.BirthDate;
        g.ContractType = req.ContractType; g.MonthlyBaseSalary = req.MonthlyBaseSalary;
        g.UpdatedDate = DateTime.Now;

        await _db.SaveChangesAsync();
        return ToDto(g, null, 0);
    }

    public async Task DeleteGuideAsync(int id)
    {
        var g = await _db.HotelTourGuides.FindAsync(id);
        if (g != null) { g.IsDeleted = true; g.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task ToggleGuideStatusAsync(int id, bool isActive)
    {
        var g = await _db.HotelTourGuides.FindAsync(id);
        if (g != null) { g.IsActive = isActive; g.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    // ── Contracts ──────────────────────────────────────────────
    public async Task<List<GuideContractDto>> GetContractsAsync(string hotelCode, int? guideId = null)
    {
        var q = _db.PmsTourGuideContracts.Include(c => c.Guide)
            .Where(c => c.HotelCode == hotelCode);
        if (guideId.HasValue) q = q.Where(c => c.GuideId == guideId.Value);
        return await q.OrderByDescending(c => c.StartDate)
            .Select(c => new GuideContractDto
            {
                Id = c.Id, GuideId = c.GuideId, GuideName = c.Guide!.Name,
                ContractCode = c.ContractCode, ContractType = c.ContractType,
                StartDate = c.StartDate, EndDate = c.EndDate,
                BasicSalary = c.BasicSalary, DailyRate = c.DailyRate,
                Status = c.Status, Notes = c.Notes, CreatedAt = c.CreatedDate
            }).ToListAsync();
    }

    public async Task<GuideContractDto> CreateContractAsync(CreateGuideContractRequest req)
    {
        // Terminate old active contracts
        var oldContracts = await _db.PmsTourGuideContracts
            .Where(c => c.HotelCode == req.HotelCode && c.GuideId == req.GuideId && c.Status == "ACTIVE")
            .ToListAsync();
        foreach (var old in oldContracts) { old.Status = "TERMINATED"; old.UpdatedDate = DateTime.Now; }

        var count = await _db.PmsTourGuideContracts.CountAsync(c => c.HotelCode == req.HotelCode);
        var contract = new PmsTourGuideContract
        {
            HotelCode = req.HotelCode, GuideId = req.GuideId,
            ContractCode = $"HDV-CTR-{count + 1:D4}",
            ContractType = req.ContractType, StartDate = req.StartDate, EndDate = req.EndDate,
            BasicSalary = req.BasicSalary, DailyRate = req.DailyRate,
            Status = "ACTIVE", Notes = req.Notes
        };
        _db.PmsTourGuideContracts.Add(contract);
        await _db.SaveChangesAsync();

        // Update guide's contract info
        var guide = await _db.HotelTourGuides.FindAsync(req.GuideId);
        if (guide != null)
        {
            guide.ContractType = req.ContractType;
            guide.MonthlyBaseSalary = req.BasicSalary;
            guide.DailyRate = req.DailyRate;
            await _db.SaveChangesAsync();
        }

        var g = await _db.HotelTourGuides.FindAsync(req.GuideId);
        return new GuideContractDto
        {
            Id = contract.Id, GuideId = contract.GuideId, GuideName = g?.Name ?? "",
            ContractCode = contract.ContractCode, ContractType = contract.ContractType,
            StartDate = contract.StartDate, EndDate = contract.EndDate,
            BasicSalary = contract.BasicSalary, DailyRate = contract.DailyRate,
            Status = contract.Status, Notes = contract.Notes, CreatedAt = contract.CreatedDate
        };
    }

    public async Task UpdateContractStatusAsync(int contractId, string status)
    {
        var c = await _db.PmsTourGuideContracts.FindAsync(contractId);
        if (c != null) { c.Status = status; c.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    // ── Salary/Payroll ─────────────────────────────────────────
    public async Task<List<GuideSalaryDto>> GetSalariesAsync(string hotelCode, int? month = null, int? year = null)
    {
        var q = _db.PmsTourGuideSalaries.Include(s => s.Guide)
            .Where(s => s.HotelCode == hotelCode);
        if (month.HasValue) q = q.Where(s => s.Month == month.Value);
        if (year.HasValue) q = q.Where(s => s.Year == year.Value);
        return await q.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month)
            .Select(s => new GuideSalaryDto
            {
                Id = s.Id, GuideId = s.GuideId, GuideName = s.Guide!.Name,
                Month = s.Month, Year = s.Year, TourCount = s.TourCount,
                DailyRate = s.DailyRate, TourIncome = s.TourIncome,
                BasicSalary = s.BasicSalary, Bonus = s.Bonus,
                Deductions = s.Deductions, TotalPay = s.TotalPay,
                Status = s.Status, PaidAt = s.PaidAt, Notes = s.Notes
            }).ToListAsync();
    }

    public async Task<GuideSalaryDto> CalculateSalaryAsync(CreateGuideSalaryRequest req)
    {
        var guide = await _db.HotelTourGuides.FindAsync(req.GuideId)
            ?? throw new InvalidOperationException("Hướng dẫn viên không tồn tại.");

        // Check existing
        var existing = await _db.PmsTourGuideSalaries.FirstOrDefaultAsync(s =>
            s.HotelCode == req.HotelCode && s.GuideId == req.GuideId && s.Month == req.Month && s.Year == req.Year);
        if (existing != null && existing.Status == "PAID")
            throw new InvalidOperationException("Tháng này đã được chi trả.");

        // Count tours in this month
        var from = new DateOnly(req.Year, req.Month, 1);
        var to = from.AddMonths(1).AddDays(-1);
        var tourCount = await _db.HotelTourSchedules
            .CountAsync(s => s.HotelCode == req.HotelCode && s.GuideId == req.GuideId && s.TourDate >= from && s.TourDate <= to);

        var tourIncome = tourCount * guide.DailyRate;
        var totalPay = guide.MonthlyBaseSalary + tourIncome + req.Bonus - req.Deductions;

        PmsTourGuideSalary salary;
        if (existing != null)
        {
            salary = existing;
        }
        else
        {
            salary = new PmsTourGuideSalary { HotelCode = req.HotelCode, GuideId = req.GuideId, Month = req.Month, Year = req.Year };
            _db.PmsTourGuideSalaries.Add(salary);
        }

        salary.TourCount = tourCount; salary.DailyRate = guide.DailyRate;
        salary.TourIncome = tourIncome; salary.BasicSalary = guide.MonthlyBaseSalary;
        salary.Bonus = req.Bonus; salary.Deductions = req.Deductions;
        salary.TotalPay = totalPay; salary.Status = "PENDING";
        salary.Notes = req.Notes; salary.UpdatedDate = DateTime.Now;
        await _db.SaveChangesAsync();

        return new GuideSalaryDto
        {
            Id = salary.Id, GuideId = salary.GuideId, GuideName = guide.Name,
            Month = salary.Month, Year = salary.Year, TourCount = salary.TourCount,
            DailyRate = salary.DailyRate, TourIncome = salary.TourIncome,
            BasicSalary = salary.BasicSalary, Bonus = salary.Bonus,
            Deductions = salary.Deductions, TotalPay = salary.TotalPay,
            Status = salary.Status, Notes = salary.Notes
        };
    }

    public async Task ApproveSalaryAsync(int salaryId)
    {
        var s = await _db.PmsTourGuideSalaries.FindAsync(salaryId);
        if (s != null && s.Status == "PENDING")
        { s.Status = "APPROVED"; s.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task MarkSalaryPaidAsync(int salaryId)
    {
        var s = await _db.PmsTourGuideSalaries.FindAsync(salaryId);
        if (s != null && s.Status == "APPROVED")
        { s.Status = "PAID"; s.PaidAt = DateTime.Now; s.UpdatedDate = DateTime.Now; await _db.SaveChangesAsync(); }
    }

    public async Task<object> GetGuideStatsAsync(string hotelCode, int guideId, int year)
    {
        var schedules = await _db.HotelTourSchedules
            .Where(s => s.HotelCode == hotelCode && s.GuideId == guideId && s.TourDate.Year == year)
            .ToListAsync();
        var salaries = await _db.PmsTourGuideSalaries
            .Where(s => s.HotelCode == hotelCode && s.GuideId == guideId && s.Year == year)
            .ToListAsync();

        return new
        {
            ToursByMonth = Enumerable.Range(1, 12).Select(m => new
            {
                Month = m,
                Count = schedules.Count(s => s.TourDate.Month == m)
            }),
            TotalTours = schedules.Count,
            TotalEarned = salaries.Sum(s => s.TotalPay),
            PaidSalaries = salaries.Count(s => s.Status == "PAID"),
            PendingSalaries = salaries.Count(s => s.Status == "PENDING" || s.Status == "APPROVED"),
        };
    }

    private static HotelTourGuideDto ToDto(HotelTourGuide g, PmsTourGuideContract? contract, int tourCount) => new()
    {
        Id = g.Id, HotelCode = g.HotelCode, GuideCode = g.GuideCode ?? "",
        Name = g.Name, Phone = g.Phone, Email = g.Email,
        Languages = g.Languages, Speciality = g.Speciality,
        IsFreelance = g.IsFreelance, DailyRate = g.DailyRate,
        Bio = g.Bio, IsActive = g.IsActive,
        HrEmployeeId = g.HrEmployeeId, IdCard = g.IdCard,
        Address = g.Address, BirthDate = g.BirthDate,
        ContractType = g.ContractType ?? "FREELANCE",
        MonthlyBaseSalary = g.MonthlyBaseSalary,
        TotalTours = tourCount,
        ContractStatus = contract?.Status,
        ContractFrom = contract?.StartDate,
        ContractTo = contract?.EndDate,
    };
}
