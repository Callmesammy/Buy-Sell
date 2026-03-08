using Application.DTOs.Order;

namespace Application.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Create order from cart or custom items
    /// </summary>
    Task<OrderResponse> CreateOrderAsync(Guid buyerId, CreateOrderRequest request);

    /// <summary>
    /// Get orders for current buyer
    /// </summary>
    Task<List<OrderResponse>> GetMyOrdersAsync(Guid buyerId);

    /// <summary>
    /// Get specific order details
    /// </summary>
    Task<OrderResponse> GetOrderAsync(Guid orderId, Guid buyerId);

    /// <summary>
    /// Get order items
    /// </summary>
    Task<List<OrderItemResponse>> GetOrderItemsAsync(Guid orderId, Guid buyerId);

    /// <summary>
    /// Cancel order (only if pending)
    /// </summary>
    Task CancelOrderAsync(Guid orderId, Guid buyerId);
}
