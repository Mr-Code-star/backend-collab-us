namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record UpdateTaskSubmissionResource(
    string? Notes,
    List<CreateSubmissionLinkResource>? Links,
    List<CreateSubmissionAttachmentResource>? Attachments
);