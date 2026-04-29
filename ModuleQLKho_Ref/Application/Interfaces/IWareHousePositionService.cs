using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger.WarehouseModel;
using AciPlatform.Domain.Entities.Ledger.WareHouseEntities;

namespace AciPlatform.Application.Services.Ledger.Interfaces.WareHouses;

public interface IWareHousePositionService
{
    Task<PagingResult<WareHousePosition>> GetAll(PagingRequestModel param);
    Task<IEnumerable<WareHousePositionGetAllModel>> GetAll();

    Task<WareHousePosition> GetById(int id);

    Task<WareHousePosition> Create(WareHousePosition param);

    Task Update(WareHousePosition param);

    Task Delete(int id);
}
