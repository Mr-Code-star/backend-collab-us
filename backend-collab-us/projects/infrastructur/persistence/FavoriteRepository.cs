using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.infrastructur.persistence;

public class FavoriteRepository(AppDbContext context) : BaseRepository<Favorite>(context), IFavoriteRepository
{
    public async Task<IEnumerable<Favorite>> GetByProfileIdAsync(int profileId)
    {
        return await Context.Set<Favorite>()
            .Where(f => f.ProfileId == profileId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Favorite>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Set<Favorite>()
            .Where(f => f.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<bool> ExistsByProfileAndProjectAsync(int profileId, int projectId)
    {
        return await Context.Set<Favorite>()
            .AnyAsync(f => f.ProfileId == profileId && f.ProjectId == projectId);
    }

    public async Task<Favorite?> FindByProfileAndProjectAsync(int profileId, int projectId)
    {
        return await Context.Set<Favorite>()
            .FirstOrDefaultAsync(f => f.ProfileId == profileId && f.ProjectId == projectId);
    }

    public async Task<int> CountByProjectIdAsync(int projectId)
    {
        return await Context.Set<Favorite>()
            .CountAsync(f => f.ProjectId == projectId);
    }

    public async Task<int> CountByProfileIdAsync(int profileId)
    {
        return await Context.Set<Favorite>()
            .CountAsync(f => f.ProfileId == profileId);
    }
    
    public async Task<bool> ExistsByIdAsync(int favoriteId)
    {
        return await Context.Set<Favorite>()
            .AnyAsync(f => f.Id == favoriteId);
    }
}