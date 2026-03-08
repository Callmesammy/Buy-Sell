using Application.Common;
using Application.DTOs.Category;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Buy_Sell.Controllers;

/// <summary>
/// Categories controller for managing product categories.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(
        ICategoryService categoryService,
        ILogger<CategoriesController> logger)
    {
        _categoryService = categoryService;
        _logger = logger;
    }

    /// <summary>
    /// Get all root categories (no parent).
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of root categories</returns>
    /// <response code="200">Categories retrieved successfully</response>
    [HttpGet("root")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRootCategories(CancellationToken ct = default)
    {
        _logger.LogInformation("Get root categories request");

        try
        {
            var result = await _categoryService.GetRootCategoriesAsync(ct);

            return Ok(new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Message = "Root categories retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving root categories");
            throw;
        }
    }

    /// <summary>
    /// Get full category tree with hierarchy.
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Category hierarchy tree</returns>
    /// <response code="200">Category tree retrieved successfully</response>
    [HttpGet("tree")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryTree(CancellationToken ct = default)
    {
        _logger.LogInformation("Get category tree request");

        try
        {
            var result = await _categoryService.GetCategoryTreeAsync(ct);

            return Ok(new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Message = "Category tree retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category tree");
            throw;
        }
    }

    /// <summary>
    /// Get a category by ID.
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Category details</returns>
    /// <response code="200">Category retrieved successfully</response>
    /// <response code="404">Category not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCategoryById(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get category request - CategoryId: {CategoryId}", id);

        try
        {
            var result = await _categoryService.GetCategoryByIdAsync(id, ct);

            return Ok(new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving category");
            throw;
        }
    }

    /// <summary>
    /// Get subcategories for a parent category.
    /// </summary>
    /// <param name="parentId">Parent category ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of subcategories</returns>
    /// <response code="200">Subcategories retrieved successfully</response>
    /// <response code="404">Parent category not found</response>
    [HttpGet("{parentId}/subcategories")]
    [ProducesResponseType(typeof(ApiResponse<List<CategoryResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSubcategories(
        [FromRoute] Guid parentId,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get subcategories request - ParentId: {ParentId}", parentId);

        try
        {
            var result = await _categoryService.GetSubcategoriesAsync(parentId, ct);

            return Ok(new ApiResponse<List<CategoryResponse>>
            {
                Success = true,
                Message = "Subcategories retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving subcategories");
            throw;
        }
    }

    /// <summary>
    /// Create a new category.
    /// </summary>
    /// <param name="request">Category details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created category</returns>
    /// <response code="201">Category created successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Create category request - Name: {CategoryName}", request.Name);

        // Validate input
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Category name is required",
                Data = null
            });
        }

        try
        {
            var result = await _categoryService.CreateCategoryAsync(request, ct);

            _logger.LogInformation("Category created successfully: {CategoryId}", result.Id);

            return CreatedAtAction(nameof(GetCategoryById), new { id = result.Id }, new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category created successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            throw;
        }
    }

    /// <summary>
    /// Update a category.
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="request">Updated category details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated category</returns>
    /// <response code="200">Category updated successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Category not found</response>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CategoryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] Guid id,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Update category request - CategoryId: {CategoryId}", id);

        // Validate input
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Category name is required",
                Data = null
            });
        }

        try
        {
            var result = await _categoryService.UpdateCategoryAsync(id, request, ct);

            _logger.LogInformation("Category updated successfully: {CategoryId}", id);

            return Ok(new ApiResponse<CategoryResponse>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category");
            throw;
        }
    }

    /// <summary>
    /// Delete a category.
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Category deleted successfully</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Category not found</response>
    /// <response code="409">Cannot delete category with products</response>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> DeleteCategory(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Delete category request - CategoryId: {CategoryId}", id);

        try
        {
            await _categoryService.DeleteCategoryAsync(id, ct);

            _logger.LogInformation("Category deleted successfully: {CategoryId}", id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Category deleted successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category");
            throw;
        }
    }
}
