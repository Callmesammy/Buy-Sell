using Application.Common;
using Application.DTOs.Product;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;

namespace Infastructure.Services;

/// <summary>
/// Service for handling product operations.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IStoreRepository _storeRepository;

    public ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        IStoreRepository storeRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _storeRepository = storeRepository;
    }

    /// <summary>
    /// Creates a new product listing.
    /// </summary>
    public async Task<ProductResponse> CreateProductAsync(CreateProductRequest request, Guid sellerId, CancellationToken ct = default)
    {
        // Verify seller exists
        var seller = await _storeRepository.GetBySellerIdAsync(sellerId, ct);
        if (seller == null)
            throw new NotFoundException("Seller not found.");

        // Verify category exists
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
        if (category == null)
            throw new NotFoundException("Category not found.");

        // Create product entity
        var product = new Product
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            StoreId = seller.Id,
            IsActive = true
        };

        // Add to repository and save
        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        return MapToResponse(product);
    }

    /// <summary>
    /// Gets a product by ID.
    /// </summary>
    public async Task<ProductResponse> GetProductByIdAsync(Guid productId, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null)
            throw new NotFoundException("Product not found.");

        return MapToResponse(product);
    }

    /// <summary>
    /// Gets all active products with pagination.
    /// </summary>
    public async Task<PagedResult<ProductResponse>> GetProductsAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var pagedResult = await _productRepository.GetAllAsync(page, pageSize, ct);
        
        return new PagedResult<ProductResponse>
        {
            Items = pagedResult.Items.Select(MapToResponse).ToList(),
            TotalCount = pagedResult.TotalCount,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    /// <summary>
    /// Gets products by category with pagination.
    /// </summary>
    public async Task<PagedResult<ProductResponse>> GetProductsByCategoryAsync(Guid categoryId, int page, int pageSize, CancellationToken ct = default)
    {
        // Verify category exists
        var category = await _categoryRepository.GetByIdAsync(categoryId, ct);
        if (category == null)
            throw new NotFoundException("Category not found.");

        var pagedResult = await _productRepository.GetAllAsync(page, pageSize, ct);
        var filtered = pagedResult.Items.Where(p => p.CategoryId == categoryId).ToList();

        return new PagedResult<ProductResponse>
        {
            Items = filtered.Select(MapToResponse).ToList(),
            TotalCount = filtered.Count,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Gets products by store (seller) with pagination.
    /// </summary>
    public async Task<PagedResult<ProductResponse>> GetProductsByStoreAsync(Guid storeId, int page, int pageSize, CancellationToken ct = default)
    {
        // Verify store exists
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
            throw new NotFoundException("Store not found.");

        var pagedResult = await _productRepository.GetByStoreIdAsync(storeId, page, pageSize, ct);

        return new PagedResult<ProductResponse>
        {
            Items = pagedResult.Items.Select(MapToResponse).ToList(),
            TotalCount = pagedResult.TotalCount,
            Page = pagedResult.Page,
            PageSize = pagedResult.PageSize
        };
    }

    /// <summary>
    /// Updates a product listing.
    /// </summary>
    public async Task<ProductResponse> UpdateProductAsync(Guid productId, CreateProductRequest request, Guid sellerId, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null)
            throw new NotFoundException("Product not found.");

        // Verify seller owns this product
        if (product.StoreId != sellerId)
            throw new UnauthorizedException("You are not authorized to update this product.");

        // Verify category exists
        if (request.CategoryId != product.CategoryId)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, ct);
            if (category == null)
                throw new NotFoundException("Category not found.");
        }

        // Update product
        product.Title = request.Title;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;

        await _productRepository.UpdateAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        return MapToResponse(product);
    }

    /// <summary>
    /// Deletes (soft delete) a product.
    /// </summary>
    public async Task DeleteProductAsync(Guid productId, Guid sellerId, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null)
            throw new NotFoundException("Product not found.");

        // Verify seller owns this product
        if (product.StoreId != sellerId)
            throw new UnauthorizedException("You are not authorized to delete this product.");

        await _productRepository.DeleteAsync(productId, ct);
        await _productRepository.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Searches products by title.
    /// </summary>
    public async Task<PagedResult<ProductResponse>> SearchProductsAsync(string query, int page, int pageSize, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Search query cannot be empty.", nameof(query));

        var pagedResult = await _productRepository.GetAllAsync(page, pageSize, ct);
        var filtered = pagedResult.Items
            .Where(p => p.Title.Contains(query, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return new PagedResult<ProductResponse>
        {
            Items = filtered.Select(MapToResponse).ToList(),
            TotalCount = filtered.Count,
            Page = page,
            PageSize = pageSize
        };
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            StoreId = product.StoreId,
            IsActive = product.IsActive,
            CreatedAt = product.CreatedAt
        };
    }
}
