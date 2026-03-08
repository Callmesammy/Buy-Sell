namespace Application.Interfaces;

/// <summary>
/// Interface for review repository operations.
/// </summary>
public interface IReviewRepository
{
    /// <summary>
    /// Gets a review by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Review?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets reviews for a product.
    /// </summary>
    Task<List<Domain.Entities.Review>> GetByProductIdAsync(Guid productId, CancellationToken ct = default);

    /// <summary>
    /// Gets reviews by a buyer.
    /// </summary>
    Task<List<Domain.Entities.Review>> GetByBuyerIdAsync(Guid buyerId, CancellationToken ct = default);

    /// <summary>
    /// Get a specific review by product and buyer.
    /// </summary>
    Task<Domain.Entities.Review?> GetReviewAsync(Guid productId, Guid buyerId, CancellationToken ct = default);

    /// <summary>
    /// Adds a new review to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Review review, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing review.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Review review, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
