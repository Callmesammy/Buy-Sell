namespace Application.Interfaces;

/// <summary>
/// Interface for order repository operations.
/// </summary>
public interface IOrderRepository
{
    /// <summary>
    /// Gets an order by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Order?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets orders by buyer ID with pagination.
    /// </summary>
    Task<Common.PagedResult<Domain.Entities.Order>> GetByBuyerIdAsync(
        Guid buyerId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    /// <summary>
    /// Gets orders by store ID (seller's incoming orders) with pagination.
    /// </summary>
    Task<Common.PagedResult<Domain.Entities.Order>> GetByStoreIdAsync(
        Guid storeId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    /// <summary>
    /// Adds a new order to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Order order, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Order order, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
