using AciPlatform.Application.DTOs;
using AciPlatform.Application.Interfaces.RestaurantErp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.RestaurantErp;

[Authorize]
[ApiController]
[Route("api/v1/restaurant-erp")]
public class RestaurantErpController : ControllerBase
{
    private readonly IRestaurantErpService _service;

    public RestaurantErpController(IRestaurantErpService service)
    {
        _service = service;
    }

    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard([FromQuery] string? companyCode)
        => Ok(await _service.GetDashboardAsync(ResolveCompany(companyCode)));

    [HttpGet("funds")]
    public async Task<IActionResult> Funds([FromQuery] string? companyCode)
        => Ok(await _service.GetFundsAsync(ResolveCompany(companyCode)));

    [HttpPost("funds")]
    public async Task<IActionResult> CreateFund([FromBody] RestaurantFundRequest request)
        => Ok(await _service.CreateFundAsync(WithCompany(request, request.CompanyCode)));

    [HttpPut("funds/{id:int}")]
    public async Task<IActionResult> UpdateFund(int id, [FromBody] RestaurantFundRequest request)
        => Ok(await _service.UpdateFundAsync(id, WithCompany(request, request.CompanyCode)));

    [HttpDelete("funds/{id:int}")]
    public async Task<IActionResult> DeleteFund(int id)
    {
        await _service.DeleteFundAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("capital-contributions")]
    public async Task<IActionResult> CapitalContributions([FromQuery] string? companyCode)
        => Ok(await _service.GetCapitalContributionsAsync(ResolveCompany(companyCode)));

    [HttpPost("capital-contributions")]
    public async Task<IActionResult> CreateCapitalContribution([FromBody] CapitalContributionRequest request)
        => Ok(await _service.CreateCapitalContributionAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("capital-contributions/{id:int}")]
    public async Task<IActionResult> UpdateCapitalContribution(int id, [FromBody] CapitalContributionRequest request)
        => Ok(await _service.UpdateCapitalContributionAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("capital-contributions/{id:int}")]
    public async Task<IActionResult> DeleteCapitalContribution(int id)
    {
        await _service.DeleteCapitalContributionAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("setup-expenses")]
    public async Task<IActionResult> SetupExpenses([FromQuery] string? companyCode)
        => Ok(await _service.GetSetupExpensesAsync(ResolveCompany(companyCode)));

    [HttpPost("setup-expenses")]
    public async Task<IActionResult> CreateSetupExpense([FromBody] SetupExpenseRequest request)
        => Ok(await _service.CreateSetupExpenseAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("setup-expenses/{id:int}")]
    public async Task<IActionResult> UpdateSetupExpense(int id, [FromBody] SetupExpenseRequest request)
        => Ok(await _service.UpdateSetupExpenseAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("setup-expenses/{id:int}")]
    public async Task<IActionResult> DeleteSetupExpense(int id)
    {
        await _service.DeleteSetupExpenseAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("material-groups")]
    public async Task<IActionResult> MaterialGroups([FromQuery] string? companyCode)
        => Ok(await _service.GetMaterialGroupsAsync(ResolveCompany(companyCode)));

    [HttpPost("material-groups")]
    public async Task<IActionResult> CreateMaterialGroup([FromBody] MaterialGroupRequest request)
        => Ok(await _service.CreateMaterialGroupAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("material-groups/{id:int}")]
    public async Task<IActionResult> UpdateMaterialGroup(int id, [FromBody] MaterialGroupRequest request)
        => Ok(await _service.UpdateMaterialGroupAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("material-groups/{id:int}")]
    public async Task<IActionResult> DeleteMaterialGroup(int id)
    {
        await _service.DeleteMaterialGroupAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("materials")]
    public async Task<IActionResult> Materials([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetMaterialsAsync(WithCompany(filter)));

    [HttpPost("materials")]
    public async Task<IActionResult> CreateMaterial([FromBody] MaterialRequest request)
        => Ok(await _service.CreateMaterialAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("materials/{id:int}")]
    public async Task<IActionResult> UpdateMaterial(int id, [FromBody] MaterialRequest request)
        => Ok(await _service.UpdateMaterialAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("materials/{id:int}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        await _service.DeleteMaterialAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("purchase-requests")]
    public async Task<IActionResult> PurchaseRequests([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetPurchaseRequestsAsync(WithCompany(filter)));

    [HttpPost("purchase-requests")]
    public async Task<IActionResult> CreatePurchaseRequest([FromBody] PurchaseRequestRequest request)
        => Ok(await _service.CreatePurchaseRequestAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("purchase-requests/{id:int}")]
    public async Task<IActionResult> UpdatePurchaseRequest(int id, [FromBody] PurchaseRequestRequest request)
        => Ok(await _service.UpdatePurchaseRequestAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("purchase-requests/{id:int}")]
    public async Task<IActionResult> DeletePurchaseRequest(int id)
    {
        await _service.DeletePurchaseRequestAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("purchase-requests/{id:int}/submit")]
    public async Task<IActionResult> SubmitPurchaseRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.SubmitPurchaseRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("purchase-requests/{id:int}/approve")]
    public async Task<IActionResult> ApprovePurchaseRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.ApprovePurchaseRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("purchase-requests/{id:int}/reject")]
    public async Task<IActionResult> RejectPurchaseRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.RejectPurchaseRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("purchase-requests/{id:int}/to-po")]
    public async Task<IActionResult> CreatePoFromRequest(int id, [FromBody] PurchaseOrderRequest request)
        => Ok(await _service.CreatePurchaseOrderFromRequestAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpGet("purchase-orders")]
    public async Task<IActionResult> PurchaseOrders([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetPurchaseOrdersAsync(WithCompany(filter)));

    [HttpPost("purchase-orders")]
    public async Task<IActionResult> CreatePurchaseOrder([FromBody] PurchaseOrderRequest request)
        => Ok(await _service.CreatePurchaseOrderAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("purchase-orders/{id:int}")]
    public async Task<IActionResult> UpdatePurchaseOrder(int id, [FromBody] PurchaseOrderRequest request)
        => Ok(await _service.UpdatePurchaseOrderAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("purchase-orders/{id:int}")]
    public async Task<IActionResult> DeletePurchaseOrder(int id)
    {
        await _service.DeletePurchaseOrderAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("purchase-orders/{id:int}/status")]
    public async Task<IActionResult> SetPurchaseOrderStatus(int id, [FromBody] StatusRequest request)
    {
        await _service.SetPurchaseOrderStatusAsync(id, request.Status);
        return Ok(new { success = true });
    }

    [HttpGet("goods-receipts")]
    public async Task<IActionResult> GoodsReceipts([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetGoodsReceiptsAsync(WithCompany(filter)));

    [HttpPost("goods-receipts")]
    public async Task<IActionResult> CreateGoodsReceipt([FromBody] GoodsReceiptRequest request)
        => Ok(await _service.CreateGoodsReceiptAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("goods-receipts/{id:int}")]
    public async Task<IActionResult> UpdateGoodsReceipt(int id, [FromBody] GoodsReceiptRequest request)
        => Ok(await _service.UpdateGoodsReceiptAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("goods-receipts/{id:int}")]
    public async Task<IActionResult> DeleteGoodsReceipt(int id)
    {
        await _service.DeleteGoodsReceiptAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("stock-balances")]
    public async Task<IActionResult> StockBalances([FromQuery] string? companyCode)
        => Ok(await _service.GetStockBalancesAsync(ResolveCompany(companyCode)));

    [HttpGet("payment-requests")]
    public async Task<IActionResult> PaymentRequests([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetPaymentRequestsAsync(WithCompany(filter)));

    [HttpPost("payment-requests")]
    public async Task<IActionResult> CreatePaymentRequest([FromBody] PaymentRequestRequest request)
        => Ok(await _service.CreatePaymentRequestAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("payment-requests/{id:int}")]
    public async Task<IActionResult> UpdatePaymentRequest(int id, [FromBody] PaymentRequestRequest request)
        => Ok(await _service.UpdatePaymentRequestAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("payment-requests/{id:int}")]
    public async Task<IActionResult> DeletePaymentRequest(int id)
    {
        await _service.DeletePaymentRequestAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("payment-requests/{id:int}/submit")]
    public async Task<IActionResult> SubmitPaymentRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.SubmitPaymentRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("payment-requests/{id:int}/approve")]
    public async Task<IActionResult> ApprovePaymentRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.ApprovePaymentRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("payment-requests/{id:int}/reject")]
    public async Task<IActionResult> RejectPaymentRequest(int id, [FromBody] ApprovalDecisionRequest request)
    {
        await _service.RejectPaymentRequestAsync(id, request);
        return Ok(new { success = true });
    }

    [HttpPost("payment-requests/{id:int}/disburse")]
    public async Task<IActionResult> Disburse(int id, [FromBody] DisbursementRequest request)
        => Ok(await _service.DisburseAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpGet("disbursements")]
    public async Task<IActionResult> Disbursements([FromQuery] string? companyCode)
        => Ok(await _service.GetDisbursementsAsync(ResolveCompany(companyCode)));

    [HttpPut("disbursements/{id:int}")]
    public async Task<IActionResult> UpdateDisbursement(int id, [FromBody] DisbursementRequest request)
        => Ok(await _service.UpdateDisbursementAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("disbursements/{id:int}")]
    public async Task<IActionResult> DeleteDisbursement(int id)
    {
        await _service.DeleteDisbursementAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("supplier-debts")]
    public async Task<IActionResult> SupplierDebts([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetSupplierDebtsAsync(WithCompany(filter)));

    [HttpPost("supplier-debts")]
    public async Task<IActionResult> CreateSupplierDebt([FromBody] SupplierDebtRequest request)
        => Ok(await _service.CreateSupplierDebtAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("supplier-debts/{id:int}")]
    public async Task<IActionResult> UpdateSupplierDebt(int id, [FromBody] SupplierDebtRequest request)
        => Ok(await _service.UpdateSupplierDebtAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("supplier-debts/{id:int}")]
    public async Task<IActionResult> DeleteSupplierDebt(int id)
    {
        await _service.DeleteSupplierDebtAsync(id);
        return Ok(new { success = true });
    }

    [HttpGet("customer-debts")]
    public async Task<IActionResult> CustomerDebts([FromQuery] RestaurantErpFilter filter)
        => Ok(await _service.GetCustomerDebtsAsync(WithCompany(filter)));

    [HttpPost("customer-debts")]
    public async Task<IActionResult> CreateCustomerDebt([FromBody] CustomerDebtRequest request)
        => Ok(await _service.CreateCustomerDebtAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpPut("customer-debts/{id:int}")]
    public async Task<IActionResult> UpdateCustomerDebt(int id, [FromBody] CustomerDebtRequest request)
        => Ok(await _service.UpdateCustomerDebtAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpDelete("customer-debts/{id:int}")]
    public async Task<IActionResult> DeleteCustomerDebt(int id)
    {
        await _service.DeleteCustomerDebtAsync(id);
        return Ok(new { success = true });
    }

    [HttpPost("customer-debts/{id:int}/receipts")]
    public async Task<IActionResult> ReceiveCustomerDebt(int id, [FromBody] CustomerDebtReceiptRequest request)
        => Ok(await _service.ReceiveCustomerDebtAsync(id, request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    [HttpGet("approval-histories")]
    public async Task<IActionResult> ApprovalHistories([FromQuery] string documentType, [FromQuery] int documentId)
        => Ok(await _service.GetApprovalHistoriesAsync(documentType, documentId));

    [HttpGet("attachments")]
    public async Task<IActionResult> Attachments([FromQuery] string documentType, [FromQuery] int documentId)
        => Ok(await _service.GetAttachmentsAsync(documentType, documentId));

    [HttpPost("attachments")]
    public async Task<IActionResult> AddAttachment([FromBody] AttachmentRequest request)
        => Ok(await _service.AddAttachmentAsync(request with { CompanyCode = ResolveCompany(request.CompanyCode) }));

    private RestaurantErpFilter WithCompany(RestaurantErpFilter filter)
    {
        filter.CompanyCode = ResolveCompany(filter.CompanyCode);
        return filter;
    }

    private RestaurantFundRequest WithCompany(RestaurantFundRequest request, string? companyCode)
        => request with { CompanyCode = ResolveCompany(companyCode) };

    private string? ResolveCompany(string? explicitCompany)
    {
        if (!string.IsNullOrWhiteSpace(explicitCompany)) return explicitCompany;
        if (Request.Headers.TryGetValue("CompanyCode", out var companyCode) && !string.IsNullOrWhiteSpace(companyCode))
            return companyCode.ToString();
        if (Request.Headers.TryGetValue("companyCode", out var lowerCompanyCode) && !string.IsNullOrWhiteSpace(lowerCompanyCode))
            return lowerCompanyCode.ToString();
        if (Request.Headers.TryGetValue("dbName", out var dbName) && !string.IsNullOrWhiteSpace(dbName))
            return dbName.ToString();
        return null;
    }
}

public record StatusRequest(string Status);
