namespace backend_collab_us.task_management.domain.model.commands;

public record CreateTaskAttachmentCommand(
    string Name,
    string Type,
    string Url,
    string Icon = null
);