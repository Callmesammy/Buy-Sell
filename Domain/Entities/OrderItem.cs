using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a line item in an order.
/// </summary>
public class OrderItem : BaseEntity
{
    /// <summary>
    /// Foreign key to order.
    /// </summary>
    public Guid OrderId { get; set; }

    /// <summary>
    /// Navigation property: Order.
    /// </summary>
    public Order Order { get; set; } = null!;

    /// <summary>
    /// Foreign key to product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Navigation property: Product.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Quantity ordered.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Price of the product at the time of purchase.
    /// </summary>
    public decimal PriceAtPurchase { get; set; }
}
