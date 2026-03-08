using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a buyer's shopping cart.
/// </summary>
public class Cart : BaseEntity
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
    /// Navigation property: Items in the cart.
    /// </summary>
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
