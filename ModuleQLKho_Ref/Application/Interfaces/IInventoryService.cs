using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Domain.Entities.Ledger;

namespace AciPlatform.Application.Services.Ledger.Interfaces.Inventorys;

public interface IInventoryService
{
    List<Inventory> GetListData(PagingRequestModel param, int year);
    string Create(List<Inventory> datas);
    IEnumerable<Inventory> GetListInventory(InventoryRequestModel param);
    List<DateTime> GetListDateInventory();
}
