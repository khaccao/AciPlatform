namespace AciPlatform.Application.DTOs.Sell;

public class CreateBillRequest
{
    public int OrderId { get; set; }
    // Add additional info for billing if necessary (like tax info, real payment amount)
}

public class CancelBillRequest
{
    public int BillId { get; set; }
    public string? Reason { get; set; }
}
