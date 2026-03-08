using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for User entity operations.
/// </summary>
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a user by email address (case-insensitive).
    /// </summary>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower(), ct);
    }
}
