using AciPlatform.Application.DTOs.Ledger.BillModels;
using AciPlatform.Application.DTOs.Ledger.GoodsModels;
using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.DTOs.Ledger.SearchModels;
using AciPlatform.Application.DTOs.Ledger.WarehouseModel;
using AciPlatform.Domain.Entities.Ledger.LedgerEntities;

namespace AciPlatform.Application.Services.Ledger.Interfaces.Goods;

public interface IGoodWarehousesService
{
    Task<PagingResult<GoodWarehousesViewModel>> GetAll(SearchViewModel param);
    Task<string> SyncChartOfAccount(int year);
    Task<string> SyncTonKho(int year);
    Task<object> CompleteBill(List<BillDetailViewPaging> billDetails, bool isForce, int userId);
    Task Update(GoodWarehousesUpdateModel item, double quantityChange = 0);
    Task<GoodWarehousesUpdateModel> GetById(int id);
    Task<string> UpdatePrintedStatus(int[] ids);
    Task<ReportForBranchModel> ReportWareHouse(int warehouseId, int shelveId, int floorId, string type);
    Task Create(Ledger entity, int year);
}
