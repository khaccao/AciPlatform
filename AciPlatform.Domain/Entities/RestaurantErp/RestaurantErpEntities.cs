using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AciPlatform.Domain.Entities.BaseEntities;

namespace AciPlatform.Domain.Entities.RestaurantErp;

public static class RestaurantErpStatus
{
    public const string Draft = "Draft";
    public const string Submitted = "Submitted";
    public const string Pending = "Pending";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Cancelled = "Cancelled";
    public const string Converted = "Converted";
    public const string Completed = "Completed";
    public const string PendingDisbursement = "PendingDisbursement";
    public const string Disbursed = "Disbursed";
    public const string New = "New";
    public const string Sent = "Sent";
    public const string Confirmed = "Confirmed";
    public const string PartiallyReceived = "PartiallyReceived";
    public const string FullyReceived = "FullyReceived";
    public const string Received = "Received";
    public const string WaitingInvoice = "WaitingInvoice";
    public const string Open = "Open";
    public const string Partial = "Partial";
    public const string Closed = "Closed";
    public const string Overdue = "Overdue";
    public const string BadDebt = "BadDebt";
}

public abstract class RestaurantEntity : BaseEntity
{
    [Key]
    public int Id { get; set; }

    [MaxLength(50)]
    public string? CompanyCode { get; set; }
}

[Table("RestaurantFunds")]
public class RestaurantFund : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(50)]
    public string FundType { get; set; } = "Cash";

    [MaxLength(50)]
    public string AccountCode { get; set; } = "111";

    public decimal OpeningBalance { get; set; }
    public decimal CurrentBalance { get; set; }
    public bool IsActive { get; set; } = true;
}

[Table("RestaurantCapitalContributions")]
public class RestaurantCapitalContribution : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string ContributorName { get; set; } = string.Empty;

    public decimal CommittedAmount { get; set; }
    public decimal ContributedAmount { get; set; }
    public DateTime ContributionDate { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "Cash";

    public int? FundId { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public long? LedgerEntryId { get; set; }
}

[Table("RestaurantSetupExpenses")]
public class RestaurantSetupExpense : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ExpenseGroup { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime ExpenseDate { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Open;

    public int? PaymentRequestId { get; set; }
    public int? PurchaseRequestId { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }
}

[Table("RestaurantMaterialGroups")]
public class RestaurantMaterialGroup : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Note { get; set; }
}

[Table("RestaurantMaterials")]
public class RestaurantMaterial : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string Name { get; set; } = string.Empty;

    public int? MaterialGroupId { get; set; }

    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? PurchaseUnit { get; set; }

    public decimal ConversionRate { get; set; } = 1;
    public decimal MinStock { get; set; }
    public decimal MaxStock { get; set; }
    public decimal LastPurchasePrice { get; set; }
    public int? DefaultSupplierId { get; set; }
    public bool HasExpiryTracking { get; set; }

    [MaxLength(50)]
    public string InventoryAccountCode { get; set; } = "152";

    [MaxLength(50)]
    public string ExpenseAccountCode { get; set; } = "642";

    public bool IsActive { get; set; } = true;
}

[Table("RestaurantPurchaseRequests")]
public class RestaurantPurchaseRequest : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? RequestDepartment { get; set; }

    [MaxLength(255)]
    public string? RequestedBy { get; set; }

    public DateTime RequestDate { get; set; } = DateTime.Now;
    public DateTime? NeededDate { get; set; }

    [MaxLength(1000)]
    public string? Reason { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Draft;

    public decimal TotalEstimatedAmount { get; set; }
    public int? CreatedPurchaseOrderId { get; set; }
}

[Table("RestaurantPurchaseRequestDetails")]
public class RestaurantPurchaseRequestDetail : RestaurantEntity
{
    public int PurchaseRequestId { get; set; }
    public int MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal EstimatedUnitPrice { get; set; }
    public decimal EstimatedAmount { get; set; }

    [MaxLength(1000)]
    public string? Reason { get; set; }
}

[Table("RestaurantPurchaseOrders")]
public class RestaurantPurchaseOrder : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public int? PurchaseRequestId { get; set; }
    public int SupplierId { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;
    public DateTime? ExpectedDeliveryDate { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.New;

    public decimal SubTotal { get; set; }
    public decimal VatAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ReceivedAmount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }
}

[Table("RestaurantPurchaseOrderDetails")]
public class RestaurantPurchaseOrderDetail : RestaurantEntity
{
    public int PurchaseOrderId { get; set; }
    public int MaterialId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal VatRate { get; set; }
    public decimal VatAmount { get; set; }
    public decimal LineAmount { get; set; }
    public decimal ReceivedQuantity { get; set; }
}

[Table("RestaurantGoodsReceipts")]
public class RestaurantGoodsReceipt : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public int? PurchaseOrderId { get; set; }
    public int? SupplierId { get; set; }

    [MaxLength(50)]
    public string WarehouseCode { get; set; } = "MAIN";

    [MaxLength(255)]
    public string? WarehouseName { get; set; }

    public DateTime ReceiptDate { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Draft;

    public decimal TotalAmount { get; set; }
    public decimal DamagedAmount { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public int? SupplierDebtId { get; set; }
    public long? LedgerEntryId { get; set; }
}

[Table("RestaurantGoodsReceiptDetails")]
public class RestaurantGoodsReceiptDetail : RestaurantEntity
{
    public int GoodsReceiptId { get; set; }
    public int MaterialId { get; set; }
    public decimal OrderedQuantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal DamagedQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineAmount { get; set; }
    public DateTime? ManufactureDate { get; set; }
    public DateTime? ExpiryDate { get; set; }

    [MaxLength(100)]
    public string? LotNumber { get; set; }
}

[Table("RestaurantStockTransactions")]
public class RestaurantStockTransaction : RestaurantEntity
{
    [MaxLength(50)]
    public string TransactionType { get; set; } = "Receipt";

    [MaxLength(50)]
    public string DocumentType { get; set; } = string.Empty;

    public int DocumentId { get; set; }
    public int MaterialId { get; set; }

    [MaxLength(50)]
    public string WarehouseCode { get; set; } = "MAIN";

    public decimal QuantityIn { get; set; }
    public decimal QuantityOut { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal BalanceAfter { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.Now;

    [MaxLength(100)]
    public string? LotNumber { get; set; }

    public DateTime? ExpiryDate { get; set; }
}

[Table("RestaurantStockBalances")]
public class RestaurantStockBalance : RestaurantEntity
{
    public int MaterialId { get; set; }

    [MaxLength(50)]
    public string WarehouseCode { get; set; } = "MAIN";

    public decimal Quantity { get; set; }
    public decimal AverageUnitPrice { get; set; }
    public decimal InventoryValue { get; set; }
    public DateTime LastTransactionDate { get; set; } = DateTime.Now;

    [MaxLength(50)]
    public string StockStatus { get; set; } = "Normal";
}

[Table("RestaurantPaymentRequests")]
public class RestaurantPaymentRequest : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [MaxLength(100)]
    public string PaymentType { get; set; } = "SupplierDebt";

    public int? SupplierId { get; set; }
    public int? PurchaseOrderId { get; set; }
    public int? GoodsReceiptId { get; set; }
    public int? SupplierDebtId { get; set; }
    public int? SetupExpenseId { get; set; }

    [MaxLength(255)]
    public string? ReceiverName { get; set; }

    public DateTime RequestDate { get; set; } = DateTime.Now;
    public decimal RequestedAmount { get; set; }
    public decimal DisbursedAmount { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Draft;

    [MaxLength(50)]
    public string DebitAccountCode { get; set; } = "331";

    [MaxLength(1000)]
    public string? Reason { get; set; }
}

[Table("RestaurantPaymentRequestDetails")]
public class RestaurantPaymentRequestDetail : RestaurantEntity
{
    public int PaymentRequestId { get; set; }

    [MaxLength(255)]
    public string Content { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    [MaxLength(50)]
    public string? ReferenceType { get; set; }

    public int? ReferenceId { get; set; }
}

[Table("RestaurantDisbursements")]
public class RestaurantDisbursement : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public int PaymentRequestId { get; set; }
    public int FundId { get; set; }
    public DateTime DisbursementDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }

    [MaxLength(255)]
    public string? ReceiverName { get; set; }

    [MaxLength(255)]
    public string? PaidBy { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public long? LedgerEntryId { get; set; }
}

[Table("RestaurantSupplierDebts")]
public class RestaurantSupplierDebt : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public int SupplierId { get; set; }
    public int? PurchaseOrderId { get; set; }
    public int? GoodsReceiptId { get; set; }
    public DateTime DebtDate { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Open;
}

[Table("RestaurantSupplierDebtPayments")]
public class RestaurantSupplierDebtPayment : RestaurantEntity
{
    public int SupplierDebtId { get; set; }
    public int? DisbursementId { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
}

[Table("RestaurantCustomerDebts")]
public class RestaurantCustomerDebt : RestaurantEntity
{
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    public int CustomerId { get; set; }
    public DateTime DebtDate { get; set; } = DateTime.Now;
    public DateTime? DueDate { get; set; }
    public decimal Amount { get; set; }
    public decimal ReceivedAmount { get; set; }
    public decimal RemainingAmount { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = RestaurantErpStatus.Open;

    [MaxLength(255)]
    public string? Description { get; set; }

    public long? LedgerEntryId { get; set; }
}

[Table("RestaurantCustomerDebtReceipts")]
public class RestaurantCustomerDebtReceipt : RestaurantEntity
{
    public int CustomerDebtId { get; set; }
    public int? FundId { get; set; }
    public DateTime ReceiptDate { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }

    [MaxLength(255)]
    public string? ReceivedBy { get; set; }

    public long? LedgerEntryId { get; set; }
}

[Table("RestaurantApprovalHistories")]
public class RestaurantApprovalHistory : RestaurantEntity
{
    [MaxLength(50)]
    public string DocumentType { get; set; } = string.Empty;

    public int DocumentId { get; set; }

    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? FromStatus { get; set; }

    [MaxLength(50)]
    public string? ToStatus { get; set; }

    public int? ApproverId { get; set; }

    [MaxLength(255)]
    public string? ApproverName { get; set; }

    [MaxLength(1000)]
    public string? Note { get; set; }

    public DateTime ActionDate { get; set; } = DateTime.Now;
}

[Table("RestaurantAttachments")]
public class RestaurantAttachment : RestaurantEntity
{
    [MaxLength(50)]
    public string DocumentType { get; set; } = string.Empty;

    public int DocumentId { get; set; }

    [MaxLength(255)]
    public string FileName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string FileUrl { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ContentType { get; set; }

    public long FileSize { get; set; }
}
