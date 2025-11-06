namespace backend_collab_us.task_management.domain.model.commands;

public record RemoveTaskToolCommand(
    int TaskId,
    int ProjectId,
    int ToolId
);