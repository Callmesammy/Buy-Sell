using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a product view/interaction by a user (used for AI recommendations).
/// </summary>
public class ProductView : BaseEntity
{
    /// <summary>
    /// Foreign key to product.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Navigation property: Product.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Foreign key to user who viewed the product.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Navigation property: User.
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Timestamp when the product was viewed.
    /// </summary>
    public DateTime ViewedAt { get; set; }
}
