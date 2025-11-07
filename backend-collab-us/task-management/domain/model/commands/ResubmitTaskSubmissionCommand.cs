namespace backend_collab_us.task_management.domain.model.commands;

public record ResubmitTaskSubmissionCommand(
    int SubmissionId,
    string? Notes,
    List<CreateSubmissionLinkCommand>? NewLinks,
    List<CreateSubmissionAttachmentCommand>? NewAttachments
);