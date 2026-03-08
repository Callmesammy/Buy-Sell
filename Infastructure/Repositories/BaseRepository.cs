using Application.Common;
using Application.Interfaces;
using Infastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Repositories;

/// <summary>
/// Base repository with common CRUD operations.
/// </summary>
/// <typeparam name="TEntity">Entity type</typeparam>
public abstract class BaseRepository<TEntity> where TEntity : class
{
    protected readonly ApplicationDbContext _context;

    protected BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets an entity by ID.
    /// </summary>
    public virtual async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken: ct);
    }

    /// <summary>
    /// Gets all entities.
    /// </summary>
    public virtual async Task<List<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync(ct);
    }

    /// <summary>
    /// Adds a new entity.
    /// </summary>
    public virtual async Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, ct);
    }

    /// <summary>
    /// Updates an entity.
    /// </summary>
    public virtual async Task UpdateAsync(TEntity entity, CancellationToken ct = default)
    {
        _context.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    public virtual async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }
}
