using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Review entity operations.
/// </summary>
public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets reviews by product ID.
    /// </summary>
    public async Task<List<Review>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await _context.Reviews
            .AsNoTracking()
            .Where(r => r.ProductId == productId)
            .ToListAsync(ct);
    }
}
