namespace Application.Interfaces;

/// <summary>
/// Interface for cart repository operations.
/// </summary>
public interface ICartRepository
{
    /// <summary>
    /// Gets a cart by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Cart?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets a cart by buyer ID.
    /// </summary>
    Task<Domain.Entities.Cart?> GetByBuyerIdAsync(Guid buyerId, CancellationToken ct = default);

    /// <summary>
    /// Adds a new cart to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Cart cart, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing cart.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Cart cart, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
