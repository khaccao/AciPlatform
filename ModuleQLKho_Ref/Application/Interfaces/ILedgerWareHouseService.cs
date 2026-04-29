using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger;

namespace AciPlatform.Application.Services.Ledger.Interfaces.Ledgers;

public interface ILedgerWareHouseService
{
    Task Create(List<LedgerWarehouseCreate> requests, string typePay, int customerId, bool isPrintBill, int year);

    Task<PagingResult<LedgerWarehousePaging>> GetListHistory(LedgerWarehousesRequestPaging param);

    Task<LedgerWarehouseDetail> GetDetailHistory(int id);
}
