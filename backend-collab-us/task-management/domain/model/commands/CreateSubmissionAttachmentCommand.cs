namespace backend_collab_us.task_management.domain.model.commands;

public record CreateSubmissionAttachmentCommand(
    string Name,
    string Type,
    string Url,
    long Size
);