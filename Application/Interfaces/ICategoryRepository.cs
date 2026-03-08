namespace Application.Interfaces;

/// <summary>
/// Interface for category repository operations.
/// </summary>
public interface ICategoryRepository
{
    /// <summary>
    /// Gets a category by its unique identifier.
    /// </summary>
    Task<Domain.Entities.Category?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    Task<List<Domain.Entities.Category>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets root categories (no parent).
    /// </summary>
    Task<List<Domain.Entities.Category>> GetRootCategoriesAsync(CancellationToken ct = default);

    /// <summary>
    /// Adds a new category to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.Category category, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    Task UpdateAsync(Domain.Entities.Category category, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
