using Application.DTOs.Store;
using Application.Interfaces;
using Domain.Exceptions;

namespace Infastructure.Services;

/// <summary>
/// Service for handling store operations.
/// </summary>
public class StoreService : IStoreService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IProductRepository _productRepository;
    private readonly IReviewRepository _reviewRepository;

    public StoreService(
        IStoreRepository storeRepository,
        IProductRepository productRepository,
        IReviewRepository reviewRepository)
    {
        _storeRepository = storeRepository;
        _productRepository = productRepository;
        _reviewRepository = reviewRepository;
    }

    /// <summary>
    /// Gets store details by ID.
    /// </summary>
    public async Task<StoreResponse> GetStoreByIdAsync(Guid storeId, CancellationToken ct = default)
    {
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
            throw new NotFoundException("Store not found.");

        return MapToResponse(store);
    }

    /// <summary>
    /// Gets store by seller ID.
    /// </summary>
    public async Task<StoreResponse> GetStoreBySellerIdAsync(Guid sellerId, CancellationToken ct = default)
    {
        var store = await _storeRepository.GetBySellerIdAsync(sellerId, ct);
        if (store == null)
            throw new NotFoundException("Store not found for this seller.");

        return MapToResponse(store);
    }

    /// <summary>
    /// Updates store information.
    /// </summary>
    public async Task<StoreResponse> UpdateStoreAsync(Guid storeId, Guid sellerId, UpdateStoreRequest request, CancellationToken ct = default)
    {
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
            throw new NotFoundException("Store not found.");

        // Verify seller owns this store
        if (store.SellerId != sellerId)
            throw new UnauthorizedException("You are not authorized to update this store.");

        // Update store details
        store.Name = request.Name;
        store.Description = request.Description;

        await _storeRepository.UpdateAsync(store, ct);
        await _storeRepository.SaveChangesAsync(ct);

        return MapToResponse(store);
    }

    /// <summary>
    /// Gets store rating and review statistics.
    /// </summary>
    public async Task<StoreStatsResponse> GetStoreStatsAsync(Guid storeId, CancellationToken ct = default)
    {
        var store = await _storeRepository.GetByIdAsync(storeId, ct);
        if (store == null)
            throw new NotFoundException("Store not found.");

        // Get all products for this store
        var allProducts = await _productRepository.GetByStoreIdAsync(storeId, 1, int.MaxValue, ct);
        var totalProducts = allProducts.TotalCount;

        // For now, return placeholder stats (reviews service can be expanded later)
        return new StoreStatsResponse
        {
            StoreId = store.Id,
            StoreName = store.Name,
            AverageRating = 0m,
            TotalReviews = 0,
            TotalProducts = totalProducts
        };
    }

    private static StoreResponse MapToResponse(Domain.Entities.Store store)
    {
        return new StoreResponse
        {
            Id = store.Id,
            Name = store.Name,
            Description = store.Description,
            LogoUrl = store.LogoUrl,
            SellerId = store.SellerId
        };
    }
}
