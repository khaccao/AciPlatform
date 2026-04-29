using AciPlatform.Application.DTOs.Events;
using AciPlatform.Application.Interfaces;
using AciPlatform.Application.Interfaces.Messaging;
using AciPlatform.Application.Interfaces.Sell;
using AciPlatform.Domain.Entities.Sell;

namespace AciPlatform.Application.Services.Sell;

public class BillService : IBillService
{
    private readonly IApplicationDbContext _context;
    private readonly IMessagePublisher _messagePublisher;

    public BillService(IApplicationDbContext context, IMessagePublisher messagePublisher)
    {
        _context = context;
        _messagePublisher = messagePublisher;
    }

    public async Task<int> CreateBillAsync(Order order, CancellationToken cancellationToken = default)
    {
        // Giả lập logic tạo Hóa Đơn từ Đơn hàng (Cắt giảm để biểu diễn Event-Driven)
        var billId = order.Id; // TODO: Insert into Bills table properly
        
        // 1. Lưu hóa đơn vào bảng Bill (Sell DB/Schema)
        order.Status = 1; // 1 = Completed
        order.IsPayment = true;
        order.PaymentAt = DateTime.Now;
        await _context.SaveChangesAsync(cancellationToken);

        // 2. KHÔNG GỌI LỆNH TRỪ KHO Ở ĐÂY (Giảm Dependency)
        // Thay vào đó, bắn Event cho Module Kho (Warehouse) tự lo việc trừ kho
        
        var billEvent = new BillCreatedEvent
        {
            BillId = billId,
            TotalAmount = order.TotalPricePaid,
            CreatedAt = DateTime.Now,
            Items = new List<BillItemEventDto>() // TODO: Fetch from OrderDetails
        };
        
        // Publish Event lên MessageBroker
        await _messagePublisher.PublishAsync(billEvent, "bill.created", cancellationToken);

        return billId;
    }

    public async Task CancelBillAsync(int id, CancellationToken cancellationToken = default)
    {
        // TODO: Logic hủy hóa đơn
        // await _messagePublisher.PublishAsync(new BillCancelledEvent { BillId = id });
        await Task.CompletedTask; throw new NotImplementedException();
    }
}

