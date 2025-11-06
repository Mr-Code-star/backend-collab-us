namespace backend_collab_us.task_management.domain.model.commands;

public record DeleteTaskCommand(
    int TaskId,
    int ProjectId,
    int DeletedBy // Usuario que elimina la tarea
);