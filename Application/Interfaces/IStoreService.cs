using Application.DTOs.Store;

namespace Application.Interfaces;

/// <summary>
/// Service for store-related operations.
/// </summary>
public interface IStoreService
{
    /// <summary>
    /// Gets store details by ID.
    /// </summary>
    Task<StoreResponse> GetStoreByIdAsync(Guid storeId, CancellationToken ct = default);

    /// <summary>
    /// Gets store by seller ID.
    /// </summary>
    Task<StoreResponse> GetStoreBySellerIdAsync(Guid sellerId, CancellationToken ct = default);

    /// <summary>
    /// Updates store information.
    /// </summary>
    Task<StoreResponse> UpdateStoreAsync(Guid storeId, Guid sellerId, UpdateStoreRequest request, CancellationToken ct = default);

    /// <summary>
    /// Gets store rating and review statistics.
    /// </summary>
    Task<StoreStatsResponse> GetStoreStatsAsync(Guid storeId, CancellationToken ct = default);
}
