using AciPlatform.Application.DTOs;
using AciPlatform.Domain.Entities.QLKho;

namespace AciPlatform.Application.Interfaces.QLKho;

public interface IWareHousePositionService
{
    Task<PagingResult<WareHousePosition>> GetAll(FilterParams param);
    Task<IEnumerable<WareHousePositionGetAllModel>> GetAll();
    Task<WareHousePosition> GetById(int id);
    Task Create(WareHousePosition param);
    Task Update(WareHousePosition param);
    Task Delete(int id);
}
