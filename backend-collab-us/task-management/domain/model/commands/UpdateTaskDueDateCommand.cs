namespace backend_collab_us.task_management.domain.model.commands;

public record UpdateTaskDueDateCommand(
    int TaskId,
    int ProjectId,
    DateTime NewDueDate,
    int UpdatedBy // Usuario que actualiza la fecha
);