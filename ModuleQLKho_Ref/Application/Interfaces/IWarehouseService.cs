using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger.WarehouseModel;
using AciPlatform.Domain.Entities.Ledger;

namespace AciPlatform.Application.Services.Ledger.Interfaces.WareHouses;

public interface IWarehouseService
{
    IEnumerable<Warehouse> GetAll();

    Task<PagingResult<WarehousePaging>> GetAll(DepartmentRequest param);

    Task<WarehouseSetterModel> GetById(int id);

    Task Create(WarehouseSetterModel param, int userId, int yearFilter);

    Task Update(WarehouseSetterModel param, int userId, int yearFilter);

    Task Delete(int id);
}
