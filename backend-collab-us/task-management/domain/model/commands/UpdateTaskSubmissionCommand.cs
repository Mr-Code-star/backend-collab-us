namespace backend_collab_us.task_management.domain.model.commands;

public record UpdateTaskSubmissionCommand(
    int SubmissionId,
    string? Notes,
    List<CreateSubmissionLinkCommand>? Links,
    List<CreateSubmissionAttachmentCommand>? Attachments
);