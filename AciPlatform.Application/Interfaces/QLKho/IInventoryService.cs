using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IInventoryService
{
    Task<IEnumerable<Inventory>> GetListData(FilterParams param, int year);
    Task Create(List<Inventory> datas);
    Task<IEnumerable<Inventory>> GetListInventory(DateTime? dtMax);
    Task<List<DateTime?>> GetListDateInventory();
    Task Accept(List<Inventory> datas);
}
