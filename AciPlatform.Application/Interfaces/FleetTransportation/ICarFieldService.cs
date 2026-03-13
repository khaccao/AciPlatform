using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface ICarFieldService
{
    Task Create(CarFieldModel param);
    Task Delete(int id);
    Task<CarFieldModel> GetById(int id);
    Task<PagingResult<CarFieldPagingModel>> GetPaging(FilterParams param);
    Task Update(CarFieldModel param);
}
