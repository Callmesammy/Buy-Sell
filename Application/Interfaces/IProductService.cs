using Application.Common;
using Application.DTOs.Product;

namespace Application.Interfaces;

/// <summary>
/// Service for product-related operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Creates a new product listing.
    /// </summary>
    Task<ProductResponse> CreateProductAsync(CreateProductRequest request, Guid sellerId, CancellationToken ct = default);

    /// <summary>
    /// Gets a product by ID.
    /// </summary>
    Task<ProductResponse> GetProductByIdAsync(Guid productId, CancellationToken ct = default);

    /// <summary>
    /// Gets all active products with pagination.
    /// </summary>
    Task<PagedResult<ProductResponse>> GetProductsAsync(int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Gets products by category with pagination.
    /// </summary>
    Task<PagedResult<ProductResponse>> GetProductsByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Gets products by store (seller) with pagination.
    /// </summary>
    Task<PagedResult<ProductResponse>> GetProductsByStoreAsync(Guid storeId, int page, int pageSize, CancellationToken ct = default);

    /// <summary>
    /// Updates a product listing.
    /// </summary>
    Task<ProductResponse> UpdateProductAsync(Guid productId, CreateProductRequest request, Guid sellerId, CancellationToken ct = default);

    /// <summary>
    /// Deletes (soft delete) a product.
    /// </summary>
    Task DeleteProductAsync(Guid productId, Guid sellerId, CancellationToken ct = default);

    /// <summary>
    /// Searches products by title.
    /// </summary>
    Task<PagedResult<ProductResponse>> SearchProductsAsync(string query, int page, int pageSize, CancellationToken ct = default);
}
