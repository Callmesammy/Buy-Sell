using Application.Common;
using Application.DTOs.Product;
using Application.Interfaces;
using Application.Validators.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Buy_Sell.Controllers;

/// <summary>
/// Products controller for managing product listings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly CreateProductValidator _createProductValidator;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(
        IProductService productService,
        CreateProductValidator createProductValidator,
        ILogger<ProductsController> logger)
    {
        _productService = productService;
        _createProductValidator = createProductValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all products with pagination.
    /// </summary>
    /// <param name="page">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 20)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated list of products</returns>
    /// <response code="200">Products retrieved successfully</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get products request - Page: {Page}, PageSize: {PageSize}", page, pageSize);

        try
        {
            var result = await _productService.GetProductsAsync(page, pageSize, ct);

            return Ok(new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products");
            throw;
        }
    }

    /// <summary>
    /// Get a product by ID.
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Product details</returns>
    /// <response code="200">Product retrieved successfully</response>
    /// <response code="404">Product not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get product request - ProductId: {ProductId}", id);

        try
        {
            var result = await _productService.GetProductByIdAsync(id, ct);

            return Ok(new ApiResponse<ProductResponse>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving product");
            throw;
        }
    }

    /// <summary>
    /// Get products by category.
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Paginated products in category</returns>
    /// <response code="200">Products retrieved successfully</response>
    /// <response code="404">Category not found</response>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductsByCategory(
        [FromRoute] Guid categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Get products by category - CategoryId: {CategoryId}", categoryId);

        try
        {
            var result = await _productService.GetProductsByCategoryAsync(categoryId, page, pageSize, ct);

            return Ok(new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Products retrieved successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving products by category");
            throw;
        }
    }

    /// <summary>
    /// Search products by title.
    /// </summary>
    /// <param name="query">Search query</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Search results</returns>
    /// <response code="200">Search completed</response>
    /// <response code="400">Invalid query</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<ProductResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SearchProducts(
        [FromQuery] string query,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Search products - Query: {Query}", query);

        try
        {
            var result = await _productService.SearchProductsAsync(query, page, pageSize, ct);

            return Ok(new ApiResponse<PagedResult<ProductResponse>>
            {
                Success = true,
                Message = "Search completed successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching products");
            throw;
        }
    }

    /// <summary>
    /// Create a new product listing.
    /// </summary>
    /// <param name="request">Product details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Created product</returns>
    /// <response code="201">Product created successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Create product request received");

        // Validate request
        var validationResult = await _createProductValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        try
        {
            var sellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
            var result = await _productService.CreateProductAsync(request, sellerId, ct);

            _logger.LogInformation("Product created successfully: {ProductId}", result.Id);

            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, new ApiResponse<ProductResponse>
            {
                Success = true,
                Message = "Product created successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            throw;
        }
    }

    /// <summary>
    /// Update a product listing.
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="request">Updated product details</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Updated product</returns>
    /// <response code="200">Product updated successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Product not found</response>
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(
        [FromRoute] Guid id,
        [FromBody] CreateProductRequest request,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Update product request - ProductId: {ProductId}", id);

        // Validate request
        var validationResult = await _createProductValidator.ValidateAsync(request, ct);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "Validation failed",
                Data = errors
            });
        }

        try
        {
            var sellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
            var result = await _productService.UpdateProductAsync(id, request, sellerId, ct);

            _logger.LogInformation("Product updated successfully: {ProductId}", id);

            return Ok(new ApiResponse<ProductResponse>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product");
            throw;
        }
    }

    /// <summary>
    /// Delete a product listing.
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Deletion confirmation</returns>
    /// <response code="200">Product deleted successfully</response>
    /// <response code="401">Unauthorized</response>
    /// <response code="404">Product not found</response>
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(
        [FromRoute] Guid id,
        CancellationToken ct = default)
    {
        _logger.LogInformation("Delete product request - ProductId: {ProductId}", id);

        try
        {
            var sellerId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new InvalidOperationException("User ID not found in claims"));
            await _productService.DeleteProductAsync(id, sellerId, ct);

            _logger.LogInformation("Product deleted successfully: {ProductId}", id);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = null
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product");
            throw;
        }
    }
}
