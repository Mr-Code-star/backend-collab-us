namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record TaskSubmissionResource(
    int Id,
    int TaskId,
    int CollaboratorId,
    string CollaboratorName,
    DateTime SubmittedAt,
    List<SubmissionLinkResource> Links,
    List<SubmissionAttachmentResource> Attachments,
    string Notes,
    string Status,
    DateTime? ReviewedAt,
    int? ReviewerId,
    string ReviewNotes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);