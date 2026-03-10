using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IWareHouseShelvesService
{
    Task<PagingResult<WarehouseShelvesPaging>> GetAll(FilterParams param);
    Task<IEnumerable<WareHouseShelvesGetAllModel>> GetAll();
    Task<WarehouseShelvesSetterModel> GetById(int id);
    Task Create(WarehouseShelvesSetterModel param);
    Task Update(WarehouseShelvesSetterModel param);
    Task Delete(int id);
}
