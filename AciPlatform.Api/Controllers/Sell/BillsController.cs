using AciPlatform.Application.DTOs;
using AciPlatform.Application.DTOs.Sell;
using AciPlatform.Application.Interfaces.Sell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Sell;

[Authorize]
[Route("api/sell/bills")]
[ApiController]
public class BillsController : ControllerBase
{
    private readonly IBillService _billService;
    private readonly IOrderService _orderService;

    public BillsController(IBillService billService, IOrderService orderService)
    {
        _billService = billService;
        _orderService = orderService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBillFromOrder([FromBody] CreateBillRequest request, CancellationToken cancellationToken)
    {
        // 1. Fetch Order
        var order = await _orderService.GetOrderByIdAsync(request.OrderId, cancellationToken);
        if (order == null) return NotFound("Order not found");

        if (order.Status == 0) return BadRequest("Order is cancelled");

        // 2. Delegate to BillService to complete the transaction and emit Event
        var billId = await _billService.CreateBillAsync(order, cancellationToken);

        return Ok(new BaseResponseModel
        {
            Data = new { BillId = billId, Message = "Bill created and sent to warehouse asynchronously" }
        });
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelBill(int id, [FromBody] CancelBillRequest request, CancellationToken cancellationToken)
    {
        await _billService.CancelBillAsync(id, cancellationToken);
        return Ok(new BaseResponseModel { Data = "Bill cancelled successfully" });
    }

    [HttpGet("{id}/export-pdf")]
    public IActionResult ExportPdf(int id)
    {
        // TODO: Use library (e.g., DinkToPdf or iTextSharp) to generate PDF byte array
        var fakePdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 }; // %PDF
        return File(fakePdfBytes, "application/pdf", $"Bill_{id}.pdf");
    }

    [HttpGet]
    public IActionResult GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        // Example endpoint to pull data to FE
        return Ok(new BaseResponseModel
        {
            Data = new List<object>(), // Replace with actual BillResponseDto
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = 0
        });
    }
}
