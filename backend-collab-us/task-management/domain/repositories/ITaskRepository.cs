using Task = backend_collab_us.task_management.domain.model.agregates.Task;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.task_management.domain.repositories;

public interface ITaskRepository : IBaseRepository<Task>
{
    // Metodos especificos para Task basados en los queries
    
    
    // Obtener todas las tareas de un proyecto
    Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId);

    // Buscar tareas por colaborador (nombre)
    Task<IEnumerable<Task>> GetByAssignedToNameAsync(int projectId, string assignedToName);
    
    Task<IEnumerable<Task>> SearchTasksAsync(
        int projectId, 
        string? assignedToName = null, 
        string? status = null);
    
    // Tareas vencidas de un proyecto
    Task<IEnumerable<Task>> GetOverdueTasksAsync(int projectId);
    
    // Métodos para verificación y relaciones
    Task<bool> ExistsByIdAsync(int taskId);
    Task<Task?> FindByIdWithRelationsAsync(int taskId);

    // Tareas que vencen pronto (en los próximos 3 días)
    Task<IEnumerable<Task>> GetUpcomingDueTasksAsync(int projectId, int daysAhead = 3);
}