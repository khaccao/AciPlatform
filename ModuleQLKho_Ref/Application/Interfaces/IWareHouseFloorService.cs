using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger.WarehouseModel;

namespace AciPlatform.Application.Services.Ledger.Interfaces.WareHouses;

public interface IWareHouseFloorService
{
    Task<PagingResult<WarehouseFloorPaging>> GetAll(PagingRequestModel param);
    Task<IEnumerable<WareHouseFloorGetAllModel>> GetAll();

    Task<WarehouseFloorSetterModel> GetById(int id);

    Task Create(WarehouseFloorSetterModel param);

    Task Update(WarehouseFloorSetterModel param);

    Task Delete(int id);
}
