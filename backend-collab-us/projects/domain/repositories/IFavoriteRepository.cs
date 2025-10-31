using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.projects.domain.repository;

public interface IFavoriteRepository : IBaseRepository<Favorite>
{
    // Métodos específicos para Favorites
    Task<IEnumerable<Favorite>> GetByProfileIdAsync(int profileId);
    Task<IEnumerable<Favorite>> GetByProjectIdAsync(int projectId);
    
    // Métodos para verificación
    Task<bool> ExistsByProfileAndProjectAsync(int profileId, int projectId);
    Task<Favorite?> FindByProfileAndProjectAsync(int profileId, int projectId);
    
    // Método para contar favoritos
    Task<int> CountByProjectIdAsync(int projectId);
    Task<int> CountByProfileIdAsync(int profileId);
    
    Task<bool> ExistsByIdAsync(int favoriteId);

}