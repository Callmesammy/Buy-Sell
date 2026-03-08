using Application.Interfaces;
using Domain.Entities;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Repository for Category entity operations.
/// </summary>
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets root categories (no parent).
    /// </summary>
    public async Task<List<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .ToListAsync();
    }
}
