namespace backend_collab_us.task_management.domain.model.commands;

public record UpdateTaskStatusCommand(
    int TaskId,
    int ProjectId,
    string Status,
    int? Progress = null
);