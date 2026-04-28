using AciPlatform.Application.DTOs;
using AciPlatform.Application.DTOs.Sell;
using AciPlatform.Application.Interfaces.Sell;
using AciPlatform.Domain.Entities.Sell;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AciPlatform.Api.Controllers.Sell;

[Authorize]
[Route("api/sell/orders")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        if (request == null) return BadRequest("Invalid request data");

        var order = new Order
        {
            CustomerId = request.CustomerId,
            TotalPrice = request.TotalPrice,
            TotalPriceDiscount = request.TotalPriceDiscount,
            TotalPricePaid = request.TotalPricePaid,
            ShippingAddress = request.ShippingAddress,
            Tell = request.Tell,
            FullName = request.FullName,
            Email = request.Email,
            PaymentMethod = request.PaymentMethod,
            Promotion = request.Promotion,
            Status = 1 // 1: Mới tạo
        };

        var createdOrder = await _orderService.CreateOrderAsync(order, cancellationToken);

        return Ok(new BaseResponseModel
        {
            Data = new OrderResponseDto
            {
                Id = createdOrder.Id,
                CustomerId = createdOrder.CustomerId,
                TotalPricePaid = createdOrder.TotalPricePaid,
                Status = createdOrder.Status,
                Date = createdOrder.Date
            }
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderDetails(int id, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetOrderByIdAsync(id, cancellationToken);
        if (order == null) return NotFound(new { msg = "Order not found" });

        return Ok(new BaseResponseModel { Data = order });
    }

    [HttpPut("{id}/cancel")]
    public async Task<IActionResult> CancelOrder(int id, CancellationToken cancellationToken)
    {
        await _orderService.CancelOrderAsync(id, cancellationToken);
        return Ok(new BaseResponseModel { Data = "Order cancelled successfully" });
    }

    [HttpGet]
    public IActionResult GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        // Example endpoint to pull data to FE
        // This is a stub; normally you'd query _context.Orders with pagination
        return Ok(new BaseResponseModel
        {
            Data = new List<OrderResponseDto>(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalItems = 0
        });
    }
}
