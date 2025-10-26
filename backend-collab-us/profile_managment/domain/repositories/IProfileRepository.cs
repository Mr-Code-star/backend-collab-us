using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.profile_managment.domain.repositories;

public interface  IProfileRepository : IBaseRepository<Profile>
{
    
    // Metodo para encontrar en base al usernmae
    Task<Profile?> FindByUsernameAsync(string username);
    
    // Metodo para encontrar con filtros
    Task<IEnumerable<Profile>> SearchProfilesAsync(
        string? query, 
        string? role, 
        int? minScore, 
        int? maxScore);
    
    Task<Profile?> FindByUserIdAsync(int userId);
}