namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateTaskSubmissionResource(
    int TaskId,
    int CollaboratorId,
    string CollaboratorName,
    string? Notes,
    List<CreateSubmissionLinkResource>? Links,
    List<CreateSubmissionAttachmentResource>? Attachments
);