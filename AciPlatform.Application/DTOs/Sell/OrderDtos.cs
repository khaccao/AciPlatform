namespace AciPlatform.Application.DTOs.Sell;

public class CreateOrderRequest
{
    public int CustomerId { get; set; }
    public double TotalPrice { get; set; }
    public double TotalPriceDiscount { get; set; }
    public double TotalPricePaid { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Tell { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Promotion { get; set; }
    
    public List<CreateOrderItemRequest> Items { get; set; } = new();
}

public class CreateOrderItemRequest
{
    public int GoodId { get; set; }
    public double Quantity { get; set; }
    public double Price { get; set; }
}

public class OrderResponseDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public double TotalPricePaid { get; set; }
    public int Status { get; set; }
    public DateTime Date { get; set; }
}
