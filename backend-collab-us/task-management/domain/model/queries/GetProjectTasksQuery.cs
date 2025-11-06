namespace backend_collab_us.task_management.domain.model.queries;

/// <summary>
/// Query para obtener todas las tareas de un proyecto específico
/// </summary>
public record GetProjectTasksQuery(int ProjectId);