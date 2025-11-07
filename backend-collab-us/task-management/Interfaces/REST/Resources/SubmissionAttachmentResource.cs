namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record SubmissionAttachmentResource(
    int Id,
    string Name,
    string Type,
    string Url,
    long Size,
    DateTime UploadedAt
);