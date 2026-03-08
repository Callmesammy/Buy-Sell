namespace Application.DTOs.Category;

/// <summary>
/// Request DTO for updating a category.
/// </summary>
public class UpdateCategoryRequest
{
    public string Name { get; set; } = string.Empty;
}
