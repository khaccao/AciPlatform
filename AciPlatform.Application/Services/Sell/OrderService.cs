using AciPlatform.Application.DTOs.Events;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.Messaging;
using AciPlatform.Application.Interfaces.Sell;
using AciPlatform.Domain.Entities.Sell;

namespace AciPlatform.Application.Services.Sell;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public OrderService(IApplicationDbContext context, IMessagePublisher messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }

    public async Task<Order> CreateOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        order.Date = DateTime.Now;
        order.Date = DateTime.Now;
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);

        // Phát ra Event để các module khác (VD: Notification) lắng nghe
        var orderEvent = new OrderCreatedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalPrice = order.TotalPricePaid,
            CreatedAt = order.Date
        };
        
        await _messagePublisher.PublishAsync(orderEvent, "order.created", cancellationToken);

        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task CancelOrderAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await _context.Orders.FindAsync(new object[] { id }, cancellationToken);
        if (order != null)
        {
            order.Status = 0; // 0 = Canceled
            await _context.SaveChangesAsync(cancellationToken);
            
            // TODO: Bắn Event order.cancelled
        }
    }
}
