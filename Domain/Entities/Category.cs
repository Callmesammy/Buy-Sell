using Domain.Common;

namespace Domain.Entities;

/// <summary>
/// Represents a product category (supports hierarchical structure).
/// </summary>
public class Category : BaseEntity
{
    /// <summary>
    /// Category name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// URL-friendly slug for the category.
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to parent category (for nested categories).
    /// </summary>
    public Guid? ParentCategoryId { get; set; }

    /// <summary>
    /// Navigation property: Parent category.
    /// </summary>
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Navigation property: Child categories.
    /// </summary>
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <summary>
    /// Navigation property: Products in this category.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
