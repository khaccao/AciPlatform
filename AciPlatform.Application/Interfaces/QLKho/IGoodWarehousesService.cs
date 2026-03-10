using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IGoodWarehousesService
{
    Task<PagingResult<GoodWarehousesViewModel>> GetAll(SearchViewModel param);
    Task SyncChartOfAccount(int year);
}
