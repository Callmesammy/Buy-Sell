using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

/// <summary>
/// Represents a customer order.
/// </summary>
public class Order : BaseEntity
{
    /// <summary>
    /// Foreign key to the buyer (User).
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Navigation property: Buyer user.
    /// </summary>
    public User Buyer { get; set; } = null!;

    /// <summary>
    /// Order status.
    /// </summary>
    public OrderStatus Status { get; set; } = OrderStatus.Pending;

    /// <summary>
    /// Total order amount in USD.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Stripe Payment Intent ID for tracking payments.
    /// </summary>
    public string? StripePaymentIntentId { get; set; }

    /// <summary>
    /// Navigation property: Items in the order.
    /// </summary>
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
