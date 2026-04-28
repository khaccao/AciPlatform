using AciPlatform.Domain.Entities.Sell;

namespace AciPlatform.Application.Interfaces.Sell;

public interface IBillService
{
    Task<int> CreateBillAsync(Order order, CancellationToken cancellationToken = default);
    Task CancelBillAsync(int id, CancellationToken cancellationToken = default);
}
