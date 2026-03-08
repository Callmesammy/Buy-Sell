using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Cart repository implementation.
/// </summary>
public class CartRepository : BaseRepository<Cart>, ICartRepository
{
    public CartRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<Cart?> GetByBuyerIdAsync(Guid buyerId, CancellationToken ct = default)
    {
        return await Context.Carts.AsNoTracking()
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
            .FirstOrDefaultAsync(c => c.BuyerId == buyerId, ct);
    }
}
