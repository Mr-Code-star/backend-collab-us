namespace backend_collab_us.task_management.domain.model.commands;

public record AddTaskAttachmentCommand(
    int TaskId,
    int ProjectId,
    string Name,
    string Type,
    string Url,
    string? Icon = null
);
