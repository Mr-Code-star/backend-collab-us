using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly AppDbContext Context;
    
    protected BaseRepository(AppDbContext context){
        Context = context;
    }
    
    /// <inheritdoc />
    
    public async Task AddAsync(TEntity entity){
        await Context.Set<TEntity>().AddAsync(entity);
    }

    /// <inheritdoc />
    public async Task<TEntity?> FindByIdAsync(int id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }
    
    /// <inheritdoc />
    
    public void Update(TEntity entity)
    {
        Context.Set<TEntity>().Update(entity);
    }

    /// <inheritdoc />

    public void Remove(TEntity entity)
    {
        Context.Set<TEntity>().Remove(entity);
    }

    /// <inheritdoc />

    public async Task<IEnumerable<TEntity>> ListAsync()
    {
        return await Context.Set<TEntity>().ToListAsync();
    }

    public async Task<TEntity?> FindByIdAsyncsString(string id)
    {
        return await Context.Set<TEntity>().FindAsync(id);
    }
}