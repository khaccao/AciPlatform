using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.HoSoNhanSu;

namespace AciPlatform.Application.Interfaces.HoSoNhanSu;

public interface IDepartmentService
{
    Task<IEnumerable<Department>> GetAllAsync(string? companyCode = null);
    Task<Department?> GetByIdAsync(int id);
    Task<Department> CreateAsync(DepartmentRequest request);
    Task UpdateAsync(int id, DepartmentRequest request);
    Task DeleteAsync(int id);
}

public interface IPositionDetailService
{
    Task<IEnumerable<PositionDetail>> GetAllAsync(string? companyCode = null);
    Task<PositionDetail?> GetByIdAsync(int id);
    Task<PositionDetail> CreateAsync(PositionDetailRequest request);
    Task UpdateAsync(int id, PositionDetailRequest request);
    Task DeleteAsync(int id);
}

public interface IDegreeService
{
    Task<IEnumerable<Degree>> GetAllAsync();
    Task<IEnumerable<Degree>> GetByUserAsync(int userId);
    Task<Degree?> GetByIdAsync(int id);
    Task<Degree> CreateAsync(DegreeRequest request);
    Task UpdateAsync(int id, DegreeRequest request);
    Task DeleteAsync(int id);
}

public interface ICertificateService
{
    Task<IEnumerable<Certificate>> GetAllAsync();
    Task<IEnumerable<Certificate>> GetByUserAsync(int userId);
    Task<Certificate?> GetByIdAsync(int id);
    Task<Certificate> CreateAsync(CertificateRequest request);
    Task UpdateAsync(int id, CertificateRequest request);
    Task DeleteAsync(int id);
}

public interface IMajorService
{
    Task<IEnumerable<Major>> GetAllAsync();
    Task<Major?> GetByIdAsync(int id);
    Task<Major> CreateAsync(MajorRequest request);
    Task UpdateAsync(int id, MajorRequest request);
    Task DeleteAsync(int id);
}

public interface IRelativeService
{
    Task<IEnumerable<Relative>> GetAllAsync();
    Task<IEnumerable<Relative>> GetByUserAsync(int userId);
    Task<Relative?> GetByIdAsync(int id);
    Task<Relative> CreateAsync(RelativeRequest request);
    Task UpdateAsync(int id, RelativeRequest request);
    Task DeleteAsync(int id);
}

public interface IHistoryAchievementService
{
    Task<IEnumerable<HistoryAchievement>> GetAllAsync();
    Task<IEnumerable<HistoryAchievement>> GetByUserAsync(int userId);
    Task<HistoryAchievement?> GetByIdAsync(int id);
    Task<HistoryAchievement> CreateAsync(HistoryAchievementRequest request);
    Task UpdateAsync(int id, HistoryAchievementRequest request);
    Task DeleteAsync(int id);
}

public interface IDecisionTypeService
{
    Task<IEnumerable<DecisionType>> GetAllAsync();
    Task<DecisionType?> GetByIdAsync(int id);
    Task<DecisionType> CreateAsync(DecisionTypeRequest request);
    Task UpdateAsync(int id, DecisionTypeRequest request);
    Task DeleteAsync(int id);
}

public interface IDecideService
{
    Task<IEnumerable<Decide>> GetAllAsync();
    Task<IEnumerable<Decide>> GetByUserAsync(int userId);
    Task<Decide?> GetByIdAsync(int id);
    Task<Decide> CreateAsync(DecideRequest request);
    Task UpdateAsync(int id, DecideRequest request);
    Task DeleteAsync(int id);
}
