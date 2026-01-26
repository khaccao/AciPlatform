namespace AciPlatform.Application.Interfaces;

public interface IInvoiceAuthorize
{
    bool CanApproveInvoice(int userId, decimal amount);
    Task CreateInvoice(int billId);
}
