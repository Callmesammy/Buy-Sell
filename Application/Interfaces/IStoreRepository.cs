namespace Application.Interfaces;

/// <summary>
/// Interface for store repository operations.
/// </summary>
public interface IStoreRepository
{
    /// <summary>
    /// Gets a store by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Store?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets a store by seller ID.
    /// </summary>
    Task<Domain.Entities.Store?> GetBySellerIdAsync(Guid sellerId, CancellationToken ct = default);

    /// <summary>
    /// Adds a new store to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Store store, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing store.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Store store, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
