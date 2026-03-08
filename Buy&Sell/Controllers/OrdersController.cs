using Application.Common;
using Application.DTOs.Order;
using Application.Interfaces;
using Application.Validators.Order;
using Buy_Sell.Middleware;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Buy_Sell.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly CreateOrderValidator _createOrderValidator;

    public OrdersController(IOrderService orderService, CreateOrderValidator createOrderValidator)
    {
        _orderService = orderService;
        _createOrderValidator = createOrderValidator;
    }

    /// <summary>
    /// Create a new order from cart or custom items
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var validationResult = await _createOrderValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var order = await _orderService.CreateOrderAsync(buyerId, request);
        return Created(string.Empty, new ApiResponse<OrderResponse>
        {
            Success = true,
            Message = "Order created successfully",
            Data = order
        });
    }

    /// <summary>
    /// Get all orders for current user
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var orders = await _orderService.GetMyOrdersAsync(buyerId);
        return Ok(new ApiResponse<List<OrderResponse>>
        {
            Success = true,
            Message = "Orders retrieved successfully",
            Data = orders
        });
    }

    /// <summary>
    /// Get specific order details
    /// </summary>
    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var order = await _orderService.GetOrderAsync(orderId, buyerId);
        return Ok(new ApiResponse<OrderResponse>
        {
            Success = true,
            Message = "Order retrieved successfully",
            Data = order
        });
    }

    /// <summary>
    /// Get items in an order
    /// </summary>
    [HttpGet("{orderId}/items")]
    public async Task<IActionResult> GetOrderItems(Guid orderId)
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        var items = await _orderService.GetOrderItemsAsync(orderId, buyerId);
        return Ok(new ApiResponse<List<OrderItemResponse>>
        {
            Success = true,
            Message = "Order items retrieved successfully",
            Data = items
        });
    }

    /// <summary>
    /// Cancel an order (only if pending)
    /// </summary>
    [HttpPost("{orderId}/cancel")]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        var buyerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? Guid.Empty.ToString());
        await _orderService.CancelOrderAsync(orderId, buyerId);
        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Order cancelled successfully"
        });
    }
}
