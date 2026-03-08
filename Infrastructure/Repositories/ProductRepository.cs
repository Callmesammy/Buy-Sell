using Application.Interfaces;
using Application.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Product repository implementation.
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<PagedResult<Product>> GetAllAsync(
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = Context.Products.AsNoTracking();

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResult<Product>> GetByStoreIdAsync(
        Guid storeId,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default)
    {
        var query = Context.Products.AsNoTracking().Where(p => p.StoreId == storeId);

        var totalCount = await query.CountAsync(ct);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await Context.Products.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (product != null)
        {
            product.IsDeleted = true;
            Context.Products.Update(product);
        }
    }
}
