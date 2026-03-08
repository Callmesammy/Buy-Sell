using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Order entity operations.
/// </summary>
public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets orders by buyer ID with pagination.
    /// </summary>
    public async Task<PagedResult<Order>> GetByBuyerIdAsync(Guid buyerId, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.Orders.AsNoTracking().Where(o => o.BuyerId == buyerId);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Gets orders by store ID with pagination.
    /// </summary>
    public async Task<PagedResult<Order>> GetByStoreIdAsync(Guid storeId, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.Orders.AsNoTracking()
            .Where(o => o.OrderItems.Any(oi => oi.Product.StoreId == storeId));
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Order>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }
}
