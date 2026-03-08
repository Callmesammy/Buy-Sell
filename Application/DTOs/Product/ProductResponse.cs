namespace Application.DTOs.Product;

/// <summary>
/// Response DTO for product details.
/// </summary>
public class ProductResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string ImageUrls { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid CategoryId { get; set; }
    public Guid StoreId { get; set; }
    public DateTime CreatedAt { get; set; }
}
