namespace Application.Interfaces;

/// <summary>
/// Interface for product repository operations.
/// </summary>
public interface IProductRepository
{
    /// <summary>
    /// Gets a product by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Product?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets all products with pagination and filtering.
    /// </summary>
    Task<Common.PagedResult<Domain.Entities.Product>> GetAllAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    /// <summary>
    /// Gets products by store ID.
    /// </summary>
    Task<Common.PagedResult<Domain.Entities.Product>> GetByStoreIdAsync(
        Guid storeId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default);

    /// <summary>
    /// Adds a new product to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Product product, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Product product, CancellationToken ct = default);

    /// <summary>
    /// Deletes a product (soft delete).
    /// </summary>
    Task DeleteAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
