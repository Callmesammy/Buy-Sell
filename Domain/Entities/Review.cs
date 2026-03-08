using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a review/rating left by a buyer on a product.
/// </summary>
public class Review : BaseEntity
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
    /// Foreign key to buyer who left the review.
    /// </summary>
    public Guid BuyerId { get; set; }

    /// <summary>
    /// Navigation property: Buyer.
    /// </summary>
    public User Buyer { get; set; } = null!;

    /// <summary>
    /// Star rating (1-5).
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Review comment text.
    /// </summary>
    public string Comment { get; set; } = string.Empty;
}
