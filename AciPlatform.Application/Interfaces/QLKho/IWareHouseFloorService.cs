using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IWareHouseFloorService
{
    Task<PagingResult<WarehouseFloorPaging>> GetAll(FilterParams param);
    Task<IEnumerable<WareHouseFloorGetAllModel>> GetAll();
    Task<WarehouseFloorSetterModel> GetById(int id);
    Task Create(WarehouseFloorSetterModel param);
    Task Update(WarehouseFloorSetterModel param);
    Task Delete(int id);
}
