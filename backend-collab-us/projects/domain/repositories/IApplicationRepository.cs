using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.projects.domain.repositories;

public interface IApplicationRepository : IBaseRepository<model.agregates.Application>
{
    // Métodos específicos para Applications
    Task<IEnumerable<model.agregates.Application>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<model.agregates.Application>> GetByApplicantIdAsync(int applicantId);
    Task<IEnumerable<model.agregates.Application>> GetByRoleIdAsync(long roleId);
    Task<IEnumerable<model.agregates.Application>> GetByStatusAsync(string status);
    
    // Métodos para verificación de duplicados
    Task<bool> ExistsByProjectAndApplicantAsync(int projectId, int applicantId);
    Task<bool> ExistsByProjectApplicantAndRoleAsync(int projectId, int applicantId, long roleId);
    
    // Métodos para estadísticas
    Task<int> CountByProjectIdAsync(int projectId);
    Task<int> CountByApplicantIdAsync(int applicantId);
    
    // Método para obtener aplicación con relaciones
    Task<model.agregates.Application?> FindByIdWithRelationsAsync(int applicationId);
    
    Task<bool> ExistsByIdAsync(int applicationId);

}