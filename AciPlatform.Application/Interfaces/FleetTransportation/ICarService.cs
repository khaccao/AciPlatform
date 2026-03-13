using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface ICarService
{
    Task<IEnumerable<CarGetterModel>> GetList();
    Task<PagingResult<CarGetterPagingModel>> GetPaging(FilterParams param);
    Task Create(CarModel param);
    Task Update(CarModel param);
    Task Delete(int id);
    Task<CarGetterDetailModel> GetById(int id);
    Task<List<CarFieldSetupGetterModel>> GetCarFieldSetup(int carId);
    Task UpdateCarFieldSetup(int carId, List<CarFieldSetupModel> carFieldSetups);
}
