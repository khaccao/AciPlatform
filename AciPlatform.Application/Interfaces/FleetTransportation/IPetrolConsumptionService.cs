using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.FleetTransportation;

public interface IPetrolConsumptionService
{
    Task<PagingResult<PetrolConsumptionGetterModel>> GetPaging(FilterParams param);
    Task Create(PetrolConsumptionModel param);
    Task Update(PetrolConsumptionModel param);
    Task Delete(int id);
    Task<PetrolConsumptionModel> GetById(int id);
    Task<IEnumerable<PetrolConsumptionReportModel>> ReportAsync(PetrolConsumptionReportRequestModel param);
}
