using AciPlatform.Application.DTOs.Ledger;
using AciPlatform.Application.Services.Ledger;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AciPlatform.Api.Controllers.Ledger
{
    [ApiController]
    [Route("api/v1/accounting/internal")]
    public class InternalAccountingOperationsController : ControllerBase
    {
        private readonly IInternalAccountingService _internalAccountingService;

        public InternalAccountingOperationsController(IInternalAccountingService internalAccountingService)
        {
            _internalAccountingService = internalAccountingService;
        }

        /// <summary>
        /// API dành cho Nhân viên/Kế toán viên Lập Phiếu Chi
        /// </summary>
        [HttpPost("payment-vouchers")]
        public async Task<IActionResult> CreatePaymentVoucher([FromBody] PaymentVoucherRequestModel request, [FromQuery] int year = 2026)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ledgerId = await _internalAccountingService.CreatePaymentVoucherAsync(request, year);
            return Ok(new { Success = true, Message = "Lập phiếu chi thành công (Đang chờ duyệt)", LedgerId = ledgerId });
        }

        /// <summary>
        /// API dành cho Kế toán trưởng / Giám đốc Duyệt Phiếu Chi
        /// </summary>
        [HttpPost("payment-vouchers/approve")]
        public async Task<IActionResult> ApprovePaymentVoucher([FromBody] ApproveVoucherRequestModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Giả lập lấy tên người duyệt từ Token
            var approverName = "Kế toán trưởng"; 

            var success = await _internalAccountingService.ApprovePaymentVoucherAsync(request, approverName);
            if (!success)
                return NotFound(new { Success = false, Message = "Không tìm thấy phiếu chi" });

            var statusStr = request.IsApproved ? "Đã duyệt" : "Từ chối";
            return Ok(new { Success = true, Message = $"Phiếu chi {statusStr} thành công" });
        }

        /// <summary>
        /// API dành cho Nhân viên Kho Lập Phiếu Nhập Kho (báo lên Kế toán)
        /// </summary>
        [HttpPost("warehouse-receipts")]
        public async Task<IActionResult> CreateWarehouseReceipt([FromBody] WarehouseReceiptRequestModel request, [FromQuery] int year = 2026)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ledgerId = await _internalAccountingService.CreateWarehouseReceiptAsync(request, year);
            return Ok(new { Success = true, Message = "Lập phiếu nhập kho thành công, đã ghi nhận công nợ Nhà Cung Cấp", LedgerId = ledgerId });
        }
    }
}
