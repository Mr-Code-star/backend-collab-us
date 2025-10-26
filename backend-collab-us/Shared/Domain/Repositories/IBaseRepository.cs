namespace backend_collab_us.Shared.Domain.Repositories;
/// <summary>
///     Base Repository interfaces for all Repositories
/// </summary>
/// <remarks>
///     This interface defines the basic CRUD operations for all repositories
/// </remarks>
/// <typeparam name="TEntity"></typeparam>
public interface IBaseRepository<TEntity>
{
    Task                        AddAsync(TEntity entity); // Add
    Task<TEntity?>              FindByIdAsync(int id); // Search
    void                        Update(TEntity entity); // Update
    void                        Remove(TEntity entity); // Remove
    Task<IEnumerable<TEntity>> ListAsync();
    
    Task<TEntity?>              FindByIdAsyncsString(string id); // Search

}