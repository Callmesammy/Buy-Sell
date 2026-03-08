namespace Application.DTOs.Product;

/// <summary>
/// Request DTO for creating a product.
/// </summary>
public class CreateProductRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public Guid CategoryId { get; set; }
}
