using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace AciPlatform.Application.DTOs.Ledger
{
    public class PagingRequestModel
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SearchText { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public int? Status { get; set; }
        public int? Year { get; set; }
        public int? Month { get; set; }
        public bool? IsInternal { get; set; }
        public string FilterDate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class LedgerReportParam : PagingRequestModel
    {
        public string AccountCode { get; set; }
        public string CustomerCode { get; set; }
        public string DebitCode { get; set; }
        public string CreditCode { get; set; }
        public string FillterType { get; set; }
        public bool? IsNoBalance { get; set; }
    }

    public class LedgerReportParamDetail : LedgerReportParam
    {
        public string DetailCodeFirst { get; set; }
        public string DetailCodeSecond { get; set; }
    }

    public class LedgerReportModel
    {
        public long Id { get; set; }
        public string VoucherNumber { get; set; }
        public DateTime? BookDate { get; set; }
        public string DebitCode { get; set; }
        public string CreditCode { get; set; }
        public double Amount { get; set; }
        public string OrginalDescription { get; set; }
        public double Balance { get; set; }
        public double OpeningBalance { get; set; }
        public double ClosingBalance { get; set; }
    }

    public class CustomActionResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        
        public CustomActionResult() {}
        public CustomActionResult(T data) { Data = data; Success = true; }
    }

    public class InventoryProductStockViewModel
    {
        public int GoodsId { get; set; }
        public double OpeningStock { get; set; }
        public double ArisingStock { get; set; }
        public double ClosingStock { get; set; }
        public double OpeningAmount { get; set; }
        public double ArisingAmount { get; set; }
        public double ClosingAmount { get; set; }
    }

    public class AppSettingInvoice
    {
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
    }

    public class UserModel
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }

    public enum AssetsType
    {
        FixedAsset = 1,
        ToolAndInstrument = 2
    }
    
    public class ObjectReturn
    {
        public int status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
