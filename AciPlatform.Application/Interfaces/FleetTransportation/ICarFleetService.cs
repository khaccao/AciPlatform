using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface ICarFleetService
{
    Task<IEnumerable<CarFleetModel>> GetList();
    Task<PagingResult<CarFleetPagingModel>> GetPaging(FilterParams param);
    Task Create(CarFleetModel model);
    Task Update(CarFleetModel model);
    Task Delete(int id);
    Task<CarFleetModel> GetById(int id);
}
