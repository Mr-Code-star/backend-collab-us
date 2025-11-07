namespace backend_collab_us.task_management.domain.model.commands;

public record CreateTaskSubmissionCommand(
    int TaskId,
    int CollaboratorId,
    string CollaboratorName,
    string? Notes,
    List<CreateSubmissionLinkCommand>? Links,
    List<CreateSubmissionAttachmentCommand>? Attachments
);