using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Product entity operations with pagination support.
/// </summary>
public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets all products with pagination.
    /// </summary>
    public async Task<PagedResult<Product>> GetAllAsync(int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.Products.AsNoTracking().Where(p => p.IsActive);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Gets products by store ID with pagination.
    /// </summary>
    public async Task<PagedResult<Product>> GetByStoreIdAsync(Guid storeId, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var query = _context.Products.AsNoTracking().Where(p => p.StoreId == storeId && p.IsActive);
        var totalCount = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(ct);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    /// <summary>
    /// Deletes (soft delete) a product by ID.
    /// </summary>
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var product = await GetByIdAsync(id, ct);
        if (product != null)
        {
            product.IsDeleted = true;
            _context.Products.Update(product);
        }
        await Task.CompletedTask;
    }
}
