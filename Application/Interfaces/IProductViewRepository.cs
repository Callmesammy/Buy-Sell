namespace Application.Interfaces;

/// <summary>
/// Interface for product view repository operations.
/// </summary>
public interface IProductViewRepository
{
    /// <summary>
    /// Gets a product view by its unique identifier.
    /// </summary>
    Task<Domain.Entities.ProductView?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets product views by user ID (for recommendation engine).
    /// </summary>
    Task<List<Domain.Entities.ProductView>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);

    /// <summary>
    /// Gets the latest product views.
    /// </summary>
    Task<List<Domain.Entities.ProductView>> GetLatestAsync(int count, CancellationToken ct = default);

    /// <summary>
    /// Adds a new product view to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.ProductView productView, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
