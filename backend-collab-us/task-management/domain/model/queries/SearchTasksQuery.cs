namespace backend_collab_us.task_management.domain.model.queries;

/// <summary>
/// Query combinado para búsqueda avanzada de tareas
/// </summary>
public record SearchTasksQuery(
    int ProjectId,
    string? AssignedToName = null,
    string? Status = null
);