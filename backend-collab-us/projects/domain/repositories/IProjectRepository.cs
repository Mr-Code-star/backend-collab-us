using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.projects.domain.repository;

public interface IProjectRepository : IBaseRepository<Project>
{
    // Métodos específicos para Projects
    Task<IEnumerable<Project>> GetByUserIdAsync(int userId);
    Task<IEnumerable<Project>> GetByAreaAsync(string area);
    Task<IEnumerable<Project>> GetByTagAsync(string tag);
    Task<IEnumerable<Project>> GetBySkillAsync(string skill);
    Task<IEnumerable<Project>> GetByStatusAsync(string status);
    Task<IEnumerable<Project>> GetByRoleNameAsync(string roleName); // ✅ NUEVO: Filtrar por nombre de rol
    
    Task<IEnumerable<Project>> SearchProjectsAsync(
        string? query, 
        string? area, 
        string? roleName, // ✅ CORREGIDO: En lugar de academicLevel
        string? status = "published");
    
    // Métodos para verificación
    Task<bool> ExistsByTitleAndUserIdAsync(string title, int userId);
    Task<Project?> FindByIdWithRelationsAsync(int projectId);
    
    Task<bool> ExistsByIdAsync(int projectId);
    
    Task<IEnumerable<Project>> GetAllWithRelationsAsync();

}