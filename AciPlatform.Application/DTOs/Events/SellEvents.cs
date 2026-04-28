namespace AciPlatform.Application.DTOs.Events;

public class BillCreatedEvent
{
    public int BillId { get; set; }
    public double TotalAmount { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<BillItemEventDto> Items { get; set; } = new();
}

public class BillItemEventDto
{
    public int GoodId { get; set; }
    public string GoodCode { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public double Price { get; set; }
}

public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public double TotalPrice { get; set; }
    public DateTime CreatedAt { get; set; }
}
