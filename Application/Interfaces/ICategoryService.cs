using Application.Common;
using Application.DTOs.Category;

namespace Application.Interfaces;

/// <summary>
/// Service for category-related operations.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Creates a new category.
    /// </summary>
    Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default);

    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    Task<CategoryResponse> GetCategoryByIdAsync(Guid categoryId, CancellationToken ct = default);

    /// <summary>
    /// Gets all root categories (no parent).
    /// </summary>
    Task<List<CategoryResponse>> GetRootCategoriesAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets subcategories for a given parent category.
    /// </summary>
    Task<List<CategoryResponse>> GetSubcategoriesAsync(Guid parentCategoryId, CancellationToken ct = default);

    /// <summary>
    /// Updates a category.
    /// </summary>
    Task<CategoryResponse> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken ct = default);

    /// <summary>
    /// Deletes (soft delete) a category.
    /// </summary>
    Task DeleteCategoryAsync(Guid categoryId, CancellationToken ct = default);

    /// <summary>
    /// Gets category hierarchy tree.
    /// </summary>
    Task<List<CategoryResponse>> GetCategoryTreeAsync(CancellationToken ct = default);
}
