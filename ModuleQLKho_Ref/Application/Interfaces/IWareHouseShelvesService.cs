using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger.WarehouseModel;

namespace AciPlatform.Application.Services.Ledger.Interfaces.WareHouses;

public interface IWareHouseShelvesService
{
    Task<PagingResult<WareHouseShelvesPaging>> GetAll(PagingRequestModel param);
    Task<IEnumerable<WareHouseShelvesGetAllModel>> GetAll();

    Task<WarehouseShelveSetterModel> GetById(int id);

    Task Create(WarehouseShelveSetterModel param);

    Task Update(WarehouseShelveSetterModel param);

    Task Delete(int id);
}
