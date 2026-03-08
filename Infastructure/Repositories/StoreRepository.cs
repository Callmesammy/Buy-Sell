using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Store entity operations.
/// </summary>
public class StoreRepository : BaseRepository<Store>, IStoreRepository
{
    public StoreRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a store by seller ID.
    /// </summary>
    public async Task<Store?> GetBySellerIdAsync(Guid sellerId, CancellationToken ct = default)
    {
        return await _context.Stores.FindAsync(new object[] { sellerId }, cancellationToken: ct);
    }
}
