using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Cart entity operations.
/// </summary>
public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a cart by buyer ID.
    /// </summary>
    public async Task<Cart?> GetByBuyerIdAsync(Guid buyerId, CancellationToken ct = default)
    {
        return await _context.Carts.FindAsync(new object[] { buyerId }, cancellationToken: ct);
    }
}
