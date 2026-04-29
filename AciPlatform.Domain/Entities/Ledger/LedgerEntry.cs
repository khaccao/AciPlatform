using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AciPlatform.Domain.Entities.Ledger;

public class LedgerEntry
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public string Type { get; set; }
    public int Month { get; set; }
    public DateTime? BookDate { get; set; }
    public string VoucherNumber { get; set; }
    public bool IsVoucher { get; set; } = false;
    public string OrginalCode { get; set; }
    public string OrginalVoucherNumber { get; set; }
    public DateTime? OrginalBookDate { get; set; }
    public string OrginalFullName { get; set; }
    public string OrginalDescription { get; set; }
    public string OrginalDescriptionEN { get; set; }
    public string OrginalCompanyName { get; set; }
    public string OrginalAddress { get; set; }
    public string AttachVoucher { get; set; }
    public string ReferenceVoucherNumber { get; set; }
    public DateTime? ReferenceBookDate { get; set; }
    public string ReferenceFullName { get; set; }
    public string ReferenceAddress { get; set; }
    public string InvoiceCode { get; set; }
    public string InvoiceAdditionalDeclarationCode { get; set; }
    public string InvoiceNumber { get; set; }
    public string InvoiceTaxCode { get; set; }
    public string InvoiceAddress { get; set; }
    public string InvoiceSerial { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public string InvoiceName { get; set; }
    public string InvoiceProductItem { get; set; }
    public string DebitCode { get; set; }
    public string DebitWarehouse { get; set; }
    public string DebitDetailCodeFirst { get; set; }
    public string DebitDetailCodeSecond { get; set; }
    public string CreditCode { get; set; }
    public string CreditWarehouse { get; set; }
    public string CreditDetailCodeFirst { get; set; }
    public string CreditDetailCodeSecond { get; set; }
    public string ProjectCode { get; set; }
    public int DepreciaMonth { get; set; } = 0;
    public int Order { get; set; } = 0;
    public int Group { get; set; } = 0;
    public DateTime? DepreciaDuration { get; set; }
    public double Quantity { get; set; } = 0;
    public double UnitPrice { get; set; } = 0;
    public double OrginalCurrency { get; set; } = 0;
    public double ExchangeRate { get; set; } = 0;
    public double Amount { get; set; } = 0;
    public bool IsAriseMark { get; set; } = false;
    public bool IsDelete { get; set; } = false;
    public DateTime? CreateAt { get; set; }
    public DateTime? UpdateAt { get; set; }
    public DateTime? DeleteAt { get; set; }
    public long UserCreated { get; set; } = 0;
    public long UserUpdated { get; set; } = 0;
    public long UserDeleted { get; set; } = 0;
    public int IsInternal { get; set; } = 0;
    public string DebitCodeName { get; set; }
    public string DebitDetailCodeFirstName { get; set; }
    public string DebitDetailCodeSecondName { get; set; }
    public string CreditCodeName { get; set; }
    public string CreditDetailCodeFirstName { get; set; }
    public string CreditDetailCodeSecondName { get; set; }
    public string DebitWarehouseName { get; set; }
    public string CreditWarehouseName { get; set; }
    public int? BillId { get; set; }
    public int? Year { get; set; } = DateTime.UtcNow.Year;
    public int? Tab { get; set; }
    public double? PercentImportTax { get; set; }
    public double? PercentTransport { get; set; }
    public double? AmountTransport { get; set; }
    public double? AmountImportWarehouse { get; set; }
    public int? Classification { get; set; }
    public int Status { get; set; } = 0;
    public string Detail1 { get; set; }
    public string Detail2 { get; set; }


    public void CheckAndMap(LedgerEntry LedgerEntry)
    {
        Type = LedgerEntry.Type;
        Month = LedgerEntry.Month;
        BookDate = LedgerEntry.BookDate;
        VoucherNumber = LedgerEntry.VoucherNumber;
        IsVoucher = LedgerEntry.IsVoucher;
        OrginalCode = LedgerEntry.OrginalCode;
        OrginalVoucherNumber = LedgerEntry.OrginalVoucherNumber;
        OrginalBookDate = LedgerEntry.OrginalBookDate;
        OrginalFullName = LedgerEntry.OrginalFullName;
        OrginalDescription = LedgerEntry.OrginalDescription;
        OrginalDescriptionEN = LedgerEntry.OrginalDescriptionEN;
        OrginalCompanyName = LedgerEntry.OrginalCompanyName;
        OrginalAddress = LedgerEntry.OrginalAddress;
        AttachVoucher = LedgerEntry.AttachVoucher;
        ReferenceVoucherNumber = LedgerEntry.ReferenceVoucherNumber;
        ReferenceBookDate = LedgerEntry.ReferenceBookDate;
        ReferenceFullName = LedgerEntry.ReferenceFullName;
        ReferenceAddress = LedgerEntry.ReferenceAddress;
        InvoiceCode = LedgerEntry.InvoiceCode;
        InvoiceAdditionalDeclarationCode = LedgerEntry.InvoiceAdditionalDeclarationCode;
        InvoiceNumber = LedgerEntry.InvoiceNumber;
        InvoiceTaxCode = LedgerEntry.InvoiceTaxCode;
        InvoiceAddress = LedgerEntry.InvoiceAddress;
        InvoiceSerial = LedgerEntry.InvoiceSerial;
        InvoiceDate = LedgerEntry.InvoiceDate;
        InvoiceName = LedgerEntry.InvoiceName;
        InvoiceProductItem = LedgerEntry.InvoiceProductItem;
        DebitCode = LedgerEntry.DebitCode;
        DebitWarehouse = LedgerEntry.DebitWarehouse;
        DebitDetailCodeFirst = LedgerEntry.DebitDetailCodeFirst;
        DebitDetailCodeSecond = LedgerEntry.DebitDetailCodeSecond;
        CreditCode = LedgerEntry.CreditCode;
        CreditWarehouse = LedgerEntry.CreditWarehouse;
        CreditDetailCodeFirst = LedgerEntry.CreditDetailCodeFirst;
        CreditDetailCodeSecond = LedgerEntry.CreditDetailCodeSecond;
        ProjectCode = LedgerEntry.ProjectCode;
        DepreciaMonth = LedgerEntry.DepreciaMonth;
        Order = LedgerEntry.Order;
        Group = LedgerEntry.Group;
        DepreciaDuration = LedgerEntry.DepreciaDuration;
        Quantity = LedgerEntry.Quantity;
        UnitPrice = LedgerEntry.UnitPrice;
        OrginalCurrency = LedgerEntry.OrginalCurrency;
        ExchangeRate = LedgerEntry.ExchangeRate;
        Amount = LedgerEntry.Amount;
        IsAriseMark = LedgerEntry.IsAriseMark;
        IsInternal = LedgerEntry.IsInternal;
        DebitCodeName = LedgerEntry.DebitCodeName;
        DebitDetailCodeFirstName = LedgerEntry.DebitDetailCodeFirstName;
        DebitDetailCodeSecondName = LedgerEntry.DebitDetailCodeSecondName;
        CreditCodeName = LedgerEntry.CreditCodeName;
        CreditDetailCodeFirstName = LedgerEntry.CreditDetailCodeFirstName;
        CreditDetailCodeSecondName = LedgerEntry.CreditDetailCodeSecondName;
        DebitWarehouseName = LedgerEntry.DebitWarehouseName;
        CreditWarehouseName = LedgerEntry.CreditWarehouseName;
        BillId = LedgerEntry.BillId;

        AmountTransport = LedgerEntry.AmountTransport;
        PercentTransport = LedgerEntry.PercentTransport;
        Tab = LedgerEntry.Tab;
        PercentImportTax = LedgerEntry.PercentImportTax;
        AmountImportWarehouse = LedgerEntry.AmountImportWarehouse;
        UserUpdated = LedgerEntry.UserUpdated;
        UpdateAt = LedgerEntry.UpdateAt;

    }
    public LedgerEntry() { }
}




