using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Review repository implementation.
/// </summary>
public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    public ReviewRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<List<Review>> GetByProductIdAsync(Guid productId, CancellationToken ct = default)
    {
        return await Context.Reviews.AsNoTracking()
            .Where(r => r.ProductId == productId)
            .Include(r => r.Buyer)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }
}
