using AciPlatform.Domain.Entities.Sell;

namespace AciPlatform.Application.Interfaces.Sell;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CancelOrderAsync(int id, CancellationToken cancellationToken = default);
}
