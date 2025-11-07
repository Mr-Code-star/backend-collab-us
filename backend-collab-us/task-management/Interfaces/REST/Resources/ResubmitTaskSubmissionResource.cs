namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record ResubmitTaskSubmissionResource(
    string? Notes,
    List<CreateSubmissionLinkResource>? NewLinks,
    List<CreateSubmissionAttachmentResource>? NewAttachments
);