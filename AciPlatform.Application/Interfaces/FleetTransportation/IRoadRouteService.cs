using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface IRoadRouteService
{
    Task Create(RoadRouteModel form);
    Task Delete(int id);
    Task<RoadRouteModel> GetDetail(int id);
    Task<PagingResult<RoadRoutePagingModel>> GetPaging(FilterParams searchRequest);
    Task Update(RoadRouteModel form);
}
