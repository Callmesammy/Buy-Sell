using Application.DTOs.Order;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;

namespace Infastructure.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public OrderService(
        IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _cartRepository = cartRepository;
        _productRepository = productRepository;
    }

    public async Task<OrderResponse> CreateOrderAsync(Guid buyerId, CreateOrderRequest request)
    {
        List<(Guid productId, int quantity)> items;

        if (request.Items == null || request.Items.Count == 0)
        {
            // Create from cart
            var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
            if (cart == null || cart.CartItems.Count == 0)
                throw new ConflictException("Cart is empty");

            items = cart.CartItems
                .Where(ci => !ci.IsDeleted)
                .Select(ci => (ci.ProductId, ci.Quantity))
                .ToList();
        }
        else
        {
            items = request.Items.Select(i => (i.ProductId, i.Quantity)).ToList();
        }

        // Validate stock and calculate total
        decimal totalAmount = 0;
        var orderItems = new List<OrderItem>();

        foreach (var (productId, quantity) in items)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new NotFoundException($"Product {productId} not found");

            if (product.Stock < quantity)
                throw new ConflictException($"Insufficient stock for {product.Title}. Available: {product.Stock}");

            var amount = product.Price * quantity;
            totalAmount += amount;

            orderItems.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                PriceAtPurchase = product.Price
            });
        }

        // Create order
        var order = new Order
        {
            BuyerId = buyerId,
            Status = OrderStatus.Pending,
            TotalAmount = totalAmount,
            OrderItems = orderItems
        };

        await _orderRepository.AddAsync(order);
        await _orderRepository.SaveChangesAsync();

        // Clear cart if created from cart
        if (request.Items == null || request.Items.Count == 0)
        {
            var cart = await _cartRepository.GetByBuyerIdAsync(buyerId);
            if (cart != null)
            {
                cart.CartItems.Clear();
                await _cartRepository.SaveChangesAsync();
            }
        }

        return MapToResponse(order);
    }

    public async Task<List<OrderResponse>> GetMyOrdersAsync(Guid buyerId)
    {
        var pagedResult = await _orderRepository.GetByBuyerIdAsync(buyerId, 1, 100);
        return pagedResult.Items.Select(MapToResponse).ToList();
    }

    public async Task<OrderResponse> GetOrderAsync(Guid orderId, Guid buyerId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException($"Order {orderId} not found");

        if (order.BuyerId != buyerId)
            throw new UnauthorizedException("You don't have permission to view this order");

        return MapToResponse(order);
    }

    public async Task<List<OrderItemResponse>> GetOrderItemsAsync(Guid orderId, Guid buyerId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException($"Order {orderId} not found");

        if (order.BuyerId != buyerId)
            throw new UnauthorizedException("You don't have permission to view this order");

        return order.OrderItems
            .Where(oi => !oi.IsDeleted)
            .Select(oi => new OrderItemResponse
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductTitle = oi.Product?.Title ?? "Unknown",
                Quantity = oi.Quantity,
                PriceAtPurchase = oi.PriceAtPurchase,
                SubTotal = oi.Quantity * oi.PriceAtPurchase
            })
            .ToList();
    }

    public async Task CancelOrderAsync(Guid orderId, Guid buyerId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new NotFoundException($"Order {orderId} not found");

        if (order.BuyerId != buyerId)
            throw new UnauthorizedException("You don't have permission to cancel this order");

        if (order.Status != OrderStatus.Pending)
            throw new ConflictException($"Cannot cancel order with status {order.Status}");

        order.Status = OrderStatus.Cancelled;
        await _orderRepository.SaveChangesAsync();
    }

    private OrderResponse MapToResponse(Order order)
    {
        return new OrderResponse
        {
            Id = order.Id,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            StripePaymentIntentId = order.StripePaymentIntentId,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            Items = order.OrderItems
                .Where(oi => !oi.IsDeleted)
                .Select(oi => new OrderItemResponse
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductTitle = oi.Product?.Title ?? "Unknown",
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase,
                    SubTotal = oi.Quantity * oi.PriceAtPurchase
                })
                .ToList()
        };
    }
}
