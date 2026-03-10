using AciPlatform.Application.DTOs;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IGoodWarehouseExportService
{
    Task<PagingResult<GoodWarehouseExportsViewModel>> GetAll(GoodWarehouseExportRequestModel param);
    Task Delete(int billId);
}
