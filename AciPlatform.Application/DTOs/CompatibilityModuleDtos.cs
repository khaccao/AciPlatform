namespace AciPlatform.Application.DTOs;

public class CustomersSearchViewModel : FilterParams
{
    public string? Code { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}

public class CustomerListItemModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime? CreatedDate { get; set; }
}

public class CustomerCodeNameModel
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}

public class OrderSearchModel : FilterParams
{
    public int? BillId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class OrderViewModelResponse
{
    public int BillId { get; set; }
    public int TotalLineItems { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class OrderLineViewModel
{
    public int ExportId { get; set; }
    public int GoodWarehouseId { get; set; }
    public string? GoodCode { get; set; }
    public string? GoodName { get; set; }
    public double Quantity { get; set; }
    public DateTime? CreatedAt { get; set; }
}
