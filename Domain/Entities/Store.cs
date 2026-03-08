using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a seller's store.
/// </summary>
public class Store : BaseEntity
{
    /// <summary>
    /// Store name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Store description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// URL to store logo in Azure Blob Storage.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Foreign key to the seller (User).
    /// </summary>
    public Guid SellerId { get; set; }

    /// <summary>
    /// Navigation property: Seller user.
    /// </summary>
    public User Seller { get; set; } = null!;

    /// <summary>
    /// Navigation property: Products in this store.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
