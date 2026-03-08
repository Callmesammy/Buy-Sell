using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Product view repository implementation (for AI recommendations).
/// </summary>
public class ProductViewRepository : BaseRepository<ProductView>, IProductViewRepository
{
    public ProductViewRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<List<ProductView>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await Context.ProductViews.AsNoTracking()
            .Where(pv => pv.UserId == userId)
            .Include(pv => pv.Product)
            .OrderByDescending(pv => pv.ViewedAt)
            .ToListAsync(ct);
    }

    public async Task<List<ProductView>> GetLatestAsync(int count, CancellationToken ct = default)
    {
        return await Context.ProductViews.AsNoTracking()
            .Include(pv => pv.Product)
            .OrderByDescending(pv => pv.ViewedAt)
            .Take(count)
            .ToListAsync(ct);
    }
}
