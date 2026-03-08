using Application.DTOs.Category;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Infastructure.Services;

/// <summary>
/// Service for handling category operations.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    public async Task<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest request, CancellationToken ct = default)
    {
        // If parent category is specified, verify it exists
        if (request.ParentCategoryId.HasValue)
        {
            var parentCategory = await _categoryRepository.GetByIdAsync(request.ParentCategoryId.Value, ct);
            if (parentCategory == null)
                throw new NotFoundException("Parent category not found.");
        }

        // Create category entity
        var category = new Category
        {
            Name = request.Name,
            Slug = GenerateSlug(request.Name),
            ParentCategoryId = request.ParentCategoryId
        };

        // Add to repository and save
        await _categoryRepository.AddAsync(category, ct);
        await _categoryRepository.SaveChangesAsync(ct);

        return MapToResponse(category);
    }

    /// <summary>
    /// Gets a category by ID.
    /// </summary>
    public async Task<CategoryResponse> GetCategoryByIdAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        if (category == null)
            throw new NotFoundException("Category not found.");

        return MapToResponse(category);
    }

    /// <summary>
    /// Gets all root categories (no parent).
    /// </summary>
    public async Task<List<CategoryResponse>> GetRootCategoriesAsync(CancellationToken ct = default)
    {
        var categories = await _categoryRepository.GetRootCategoriesAsync(ct);
        return categories.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Gets subcategories for a given parent category.
    /// </summary>
    public async Task<List<CategoryResponse>> GetSubcategoriesAsync(Guid parentCategoryId, CancellationToken ct = default)
    {
        // Verify parent category exists
        var parentCategory = await _categoryRepository.GetByIdAsync(parentCategoryId, ct);
        if (parentCategory == null)
            throw new NotFoundException("Parent category not found.");

        // Get all categories and filter in memory for subcategories
        var allCategories = await _categoryRepository.GetAllAsync(ct);
        var subcategories = allCategories.Where(c => c.ParentCategoryId == parentCategoryId).ToList();

        return subcategories.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Updates a category.
    /// </summary>
    public async Task<CategoryResponse> UpdateCategoryAsync(Guid categoryId, UpdateCategoryRequest request, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        if (category == null)
            throw new NotFoundException("Category not found.");

        // Update category details
        category.Name = request.Name;
        category.Slug = GenerateSlug(request.Name);

        await _categoryRepository.UpdateAsync(category, ct);
        await _categoryRepository.SaveChangesAsync(ct);

        return MapToResponse(category);
    }

    /// <summary>
    /// Deletes (soft delete) a category.
    /// </summary>
    public async Task DeleteCategoryAsync(Guid categoryId, CancellationToken ct = default)
    {
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        if (category == null)
            throw new NotFoundException("Category not found.");

        // Check if category has subcategories - prevent deletion
        var allCategories = await _categoryRepository.GetAllAsync(ct);
        var hasSubcategories = allCategories.Any(c => c.ParentCategoryId == categoryId);
        if (hasSubcategories)
            throw new ConflictException("Cannot delete category with subcategories.");

        // Check if category has products - prevent deletion
        if (category.Products.Any())
            throw new ConflictException("Cannot delete category with active products.");

        // Would call DeleteAsync here if it existed in the interface
        // For now, we'll update the repository interface in the next step
        await _categoryRepository.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Gets category hierarchy tree.
    /// </summary>
    public async Task<List<CategoryResponse>> GetCategoryTreeAsync(CancellationToken ct = default)
    {
        var rootCategories = await _categoryRepository.GetRootCategoriesAsync(ct);
        var allCategories = await _categoryRepository.GetAllAsync(ct);
        
        var tree = rootCategories.Select(c => BuildCategoryTree(c, allCategories)).ToList();
        return tree;
    }

    /// <summary>
    /// Recursively builds category tree with subcategories.
    /// </summary>
    private CategoryResponse BuildCategoryTree(Category category, List<Category> allCategories)
    {
        var response = MapToResponse(category);
        
        var subcategories = allCategories
            .Where(c => c.ParentCategoryId == category.Id)
            .ToList();

        response.Subcategories = subcategories
            .Select(sub => BuildCategoryTree(sub, allCategories))
            .ToList();

        return response;
    }

    /// <summary>
    /// Generates a URL-friendly slug from category name.
    /// </summary>
    private static string GenerateSlug(string name)
    {
        return name.ToLower().Replace(" ", "-");
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = string.Empty,
            ParentCategoryId = category.ParentCategoryId,
            CreatedAt = category.CreatedAt
        };
    }
}
