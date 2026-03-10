using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IWarehouseService
{
    Task<IEnumerable<Warehouse>> GetAll();
    Task<PagingResult<WarehousePaging>> GetPaging(FilterParams param);
    Task<WarehouseSetterModel> GetById(int id);
    Task Create(WarehouseSetterModel param, int userId, int yearFilter);
    Task Update(WarehouseSetterModel param, int userId, int yearFilter);
    Task Delete(int id);
}
