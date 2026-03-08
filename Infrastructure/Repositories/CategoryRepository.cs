using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

/// <summary>
/// Category repository implementation.
/// </summary>
public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    public CategoryRepository(Infrastructure.Persistence.ApplicationDbContext context) : base(context) { }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct = default)
    {
        return await Context.Categories.AsNoTracking()
            .Include(c => c.SubCategories)
            .ToListAsync(ct);
    }

    public async Task<List<Category>> GetRootCategoriesAsync(CancellationToken ct = default)
    {
        return await Context.Categories.AsNoTracking()
            .Where(c => c.ParentCategoryId == null)
            .Include(c => c.SubCategories)
            .ToListAsync(ct);
    }
}
