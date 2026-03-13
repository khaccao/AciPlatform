using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface IPoliceCheckPointService
{
    Task Create(PoliceCheckPointModel form);
    Task Delete(int id);
    Task<IEnumerable<PoliceCheckPointModel>> GetAll();
    Task<PoliceCheckPointModel> GetDetail(int id);
    Task<PagingResult<PoliceCheckPointModel>> GetPaging(FilterParams searchRequest);
    Task Update(PoliceCheckPointModel form);
}
