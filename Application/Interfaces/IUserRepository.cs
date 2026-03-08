namespace Application.Interfaces;

/// <summary>
/// Interface for user repository operations.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their unique identifier.
    /// </summary>
    Task<Domain.Entities.User?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    Task<Domain.Entities.User?> GetByEmailAsync(string email, CancellationToken ct = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    Task AddAsync(Domain.Entities.User user, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    Task UpdateAsync(Domain.Entities.User user, CancellationToken ct = default);

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    Task SaveChangesAsync(CancellationToken ct = default);
}
