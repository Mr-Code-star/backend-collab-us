namespace backend_collab_us.task_management.domain.model.commands;

public record ToggleTaskToolCommand(
    int TaskId,
    int ProjectId,
    int ToolId
);