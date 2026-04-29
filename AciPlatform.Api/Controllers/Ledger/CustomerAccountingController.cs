using AciPlatform.Application.Services.Ledger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AciPlatform.Api.Controllers.Ledger
{
    [ApiController]
    [Route("api/v1/accounting/customers")]
    public class CustomerAccountingController : ControllerBase
    {
        private readonly ICustomerLedgerIntegrationService _customerLedgerIntegrationService;

        public CustomerAccountingController(ICustomerLedgerIntegrationService customerLedgerIntegrationService)
        {
            _customerLedgerIntegrationService = customerLedgerIntegrationService;
        }

        public class ReceiptVoucherRequest
        {
            public int CustomerId { get; set; }
            public double Amount { get; set; }
            public string PaymentMethod { get; set; }
            public int IsInternal { get; set; } = 1;
        }

        [HttpPost("receipt-vouchers")]
        public async Task<IActionResult> CreateReceiptVoucher([FromBody] ReceiptVoucherRequest request, [FromQuery] int year = 2026)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _customerLedgerIntegrationService.AutoAccountPaymentReceiptAsync(
                request.CustomerId, 
                request.Amount, 
                request.PaymentMethod, 
                year, 
                request.IsInternal);

            return Ok(new { Success = true, Message = "Lập phiếu thu và ghi giảm công nợ thành công" });
        }

        [HttpGet("{customerId}/debt")]
        public async Task<IActionResult> GetCustomerDebt(int customerId, [FromQuery] int year = 2026, [FromQuery] int isInternal = 1)
        {
            var debt = await _customerLedgerIntegrationService.GetCustomerDebtBalanceAsync(customerId, year, isInternal);
            return Ok(new { Success = true, CustomerId = customerId, DebtBalance = debt });
        }
    }
}
