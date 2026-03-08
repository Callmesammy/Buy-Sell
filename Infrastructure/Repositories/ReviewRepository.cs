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
            .Where(r => r.ProductId == productId && !r.IsDeleted)
            .Include(r => r.Buyer)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<List<Review>> GetByBuyerIdAsync(Guid buyerId, CancellationToken ct = default)
    {
        return await Context.Reviews.AsNoTracking()
            .Where(r => r.BuyerId == buyerId && !r.IsDeleted)
            .Include(r => r.Product)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Review?> GetReviewAsync(Guid productId, Guid buyerId, CancellationToken ct = default)
    {
        return await Context.Reviews.AsNoTracking()
            .Include(r => r.Product)
            .Include(r => r.Buyer)
            .FirstOrDefaultAsync(r => r.ProductId == productId && r.BuyerId == buyerId && !r.IsDeleted, ct);
    }
}
