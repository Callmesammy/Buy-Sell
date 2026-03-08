namespace Infrastructure.Repositories;

/// <summary>
/// Base repository with common CRUD operations.
/// </summary>
public abstract class BaseRepository<T> where T : Domain.Common.BaseEntity
{
    protected readonly Infrastructure.Persistence.ApplicationDbContext Context;

    protected BaseRepository(Infrastructure.Persistence.ApplicationDbContext context)
    {
        Context = context;
    }

    public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await Context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public virtual async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await Context.Set<T>().AddAsync(entity, ct);
    }

    public virtual async Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Context.Set<T>().Update(entity);
        await Task.CompletedTask;
    }

    public virtual async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await Context.SaveChangesAsync(ct);
    }
}
