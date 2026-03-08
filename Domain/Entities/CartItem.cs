using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents an item in a buyer's shopping cart.
/// </summary>
public class CartItem : BaseEntity
{
    /// <summary>
    /// Foreign key to cart.
    /// </summary>
    public Guid CartId { get; set; }

    /// <summary>
    /// Navigation property: Cart.
    /// </summary>
    public Cart Cart { get; set; } = null!;

    /// <summary>
    /// Foreign key to product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Navigation property: Product.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Quantity of the product in the cart.
    /// </summary>
    public int Quantity { get; set; }
}
