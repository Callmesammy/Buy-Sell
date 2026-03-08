namespace Application.DTOs.Category;

/// <summary>
/// Response DTO for category information.
/// </summary>
public class CategoryResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public List<CategoryResponse> Subcategories { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
