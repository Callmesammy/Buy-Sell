namespace Application.DTOs.Category;

/// <summary>
/// Request DTO for creating a new category.
/// </summary>
public class CreateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}
