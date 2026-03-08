using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a product listing.
/// </summary>
public class Product : BaseEntity
{
    /// <summary>
    /// Product title.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Product description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product price in USD.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Available stock quantity.
    /// </summary>
    public int Stock { get; set; }

    /// <summary>
    /// Comma-separated URLs of product images (Azure Blob Storage).
    /// </summary>
    public string ImageUrls { get; set; } = string.Empty;

    /// <summary>
    /// Whether the product is active/visible.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Foreign key to category.
    /// </summary>
    public Guid CategoryId { get; set; }

    /// <summary>
    /// Navigation property: Category.
    /// </summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// Foreign key to store.
    /// </summary>
    public Guid StoreId { get; set; }

    /// <summary>
    /// Navigation property: Store.
    /// </summary>
    public Store Store { get; set; } = null!;

    /// <summary>
    /// Navigation property: Reviews for this product.
    /// </summary>
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    /// <summary>
    /// Navigation property: Product views for recommendations.
    /// </summary>
    public ICollection<ProductView> ProductViews { get; set; } = new List<ProductView>();
}
