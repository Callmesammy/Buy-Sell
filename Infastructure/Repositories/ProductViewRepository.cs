using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for ProductView entity operations.
/// </summary>
public class ProductViewRepository : BaseRepository<ProductView>, IProductViewRepository
{
    public ProductViewRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets product views by user ID.
    /// </summary>
    public async Task<List<ProductView>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await _context.ProductViews
            .AsNoTracking()
            .Where(pv => pv.UserId == userId)
            .ToListAsync(ct);
    }

    /// <summary>
    /// Gets the latest product views for AI recommendations.
    /// </summary>
    public async Task<List<ProductView>> GetLatestAsync(int count, CancellationToken ct = default)
    {
        return await _context.ProductViews
            .AsNoTracking()
            .OrderByDescending(pv => pv.ViewedAt)
            .Take(count)
            .ToListAsync(ct);
    }
}
