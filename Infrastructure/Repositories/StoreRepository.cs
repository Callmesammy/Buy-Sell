using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Store repository implementation.
/// </summary>
public class StoreRepository : BaseRepository<Store>, IStoreRepository
{
    public StoreRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<Store?> GetBySellerIdAsync(Guid sellerId, CancellationToken ct = default)
    {
        return await Context.Stores.AsNoTracking()
            .FirstOrDefaultAsync(s => s.SellerId == sellerId, ct);
    }
}
