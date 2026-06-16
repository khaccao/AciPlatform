namespace AciPlatform.Application.DTOs;

public class RestaurantErpFilter
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
    public string? SearchText { get; set; }
    public string? Status { get; set; }
    public string? CompanyCode { get; set; }
}

public class RestaurantDashboardDto
{
    public decimal TotalCommittedCapital { get; set; }
    public decimal TotalContributedCapital { get; set; }
    public decimal RemainingCapitalToContribute { get; set; }
    public decimal TotalSetupExpense { get; set; }
    public decimal TotalDisbursed { get; set; }
    public decimal CashBalance { get; set; }
    public decimal BankBalance { get; set; }
    public decimal TotalFundBalance { get; set; }
    public decimal SupplierDebt { get; set; }
    public decimal CustomerDebt { get; set; }
    public int PurchaseRequestsPending { get; set; }
    public int PaymentRequestsPending { get; set; }
    public decimal ApprovedNotDisbursed { get; set; }
    public decimal InventoryValue { get; set; }
    public int LowStockMaterials { get; set; }
}

public record RestaurantFundRequest(
    string Code,
    string Name,
    string FundType,
    string AccountCode,
    decimal OpeningBalance,
    string? CompanyCode);

public record CapitalContributionRequest(
    string Code,
    string ContributorName,
    decimal CommittedAmount,
    decimal ContributedAmount,
    DateTime ContributionDate,
    string PaymentMethod,
    int? FundId,
    string? Note,
    string? CompanyCode);

public record SetupExpenseRequest(
    string Code,
    string Name,
    string ExpenseGroup,
    decimal Amount,
    DateTime ExpenseDate,
    int? PaymentRequestId,
    int? PurchaseRequestId,
    string? Note,
    string? CompanyCode);

public record MaterialGroupRequest(string Code, string Name, string? Note, string? CompanyCode);

public record MaterialRequest(
    string Code,
    string Name,
    int? MaterialGroupId,
    string Unit,
    string? PurchaseUnit,
    decimal ConversionRate,
    decimal MinStock,
    decimal MaxStock,
    decimal LastPurchasePrice,
    int? DefaultSupplierId,
    bool HasExpiryTracking,
    string InventoryAccountCode,
    string ExpenseAccountCode,
    string? CompanyCode);

public record PurchaseRequestLineRequest(int MaterialId, decimal Quantity, decimal EstimatedUnitPrice, string? Reason);

public record PurchaseRequestRequest(
    string Code,
    string? RequestDepartment,
    string? RequestedBy,
    DateTime RequestDate,
    DateTime? NeededDate,
    string? Reason,
    string? CompanyCode,
    List<PurchaseRequestLineRequest> Items);

public record ApprovalDecisionRequest(string? ApproverName, int? ApproverId, string? Note);

public record PurchaseOrderLineRequest(int MaterialId, decimal Quantity, decimal UnitPrice, decimal VatRate);

public record PurchaseOrderRequest(
    string Code,
    int? PurchaseRequestId,
    int SupplierId,
    DateTime OrderDate,
    DateTime? ExpectedDeliveryDate,
    string? Note,
    string? CompanyCode,
    List<PurchaseOrderLineRequest> Items);

public record GoodsReceiptLineRequest(
    int MaterialId,
    decimal OrderedQuantity,
    decimal ReceivedQuantity,
    decimal DamagedQuantity,
    decimal UnitPrice,
    DateTime? ManufactureDate,
    DateTime? ExpiryDate,
    string? LotNumber);

public record GoodsReceiptRequest(
    string Code,
    int? PurchaseOrderId,
    int? SupplierId,
    string WarehouseCode,
    string? WarehouseName,
    DateTime ReceiptDate,
    string Status,
    string? Note,
    string? CompanyCode,
    List<GoodsReceiptLineRequest> Items);

public record PaymentRequestLineRequest(string Content, decimal Amount, string? ReferenceType, int? ReferenceId);

public record PaymentRequestRequest(
    string Code,
    string PaymentType,
    int? SupplierId,
    int? PurchaseOrderId,
    int? GoodsReceiptId,
    int? SupplierDebtId,
    int? SetupExpenseId,
    string? ReceiverName,
    DateTime RequestDate,
    decimal RequestedAmount,
    string DebitAccountCode,
    string? Reason,
    string? CompanyCode,
    List<PaymentRequestLineRequest> Items);

public record DisbursementRequest(
    string Code,
    int FundId,
    DateTime DisbursementDate,
    decimal Amount,
    string? ReceiverName,
    string? PaidBy,
    string? Note,
    string? CompanyCode);

public record SupplierDebtRequest(
    string Code,
    int SupplierId,
    int? PurchaseOrderId,
    int? GoodsReceiptId,
    DateTime DebtDate,
    DateTime? DueDate,
    decimal Amount,
    string? CompanyCode);

public record CustomerDebtRequest(
    string Code,
    int CustomerId,
    DateTime DebtDate,
    DateTime? DueDate,
    decimal Amount,
    string? Description,
    string? CompanyCode);

public record CustomerDebtReceiptRequest(
    int? FundId,
    DateTime ReceiptDate,
    decimal Amount,
    string? ReceivedBy,
    string? CompanyCode);

public record AttachmentRequest(
    string DocumentType,
    int DocumentId,
    string FileName,
    string FileUrl,
    string? ContentType,
    long FileSize,
    string? CompanyCode);
