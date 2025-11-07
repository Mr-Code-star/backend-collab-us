namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record TaskAttachmentResource(
    int Id,
    string Name,
    string Type,
    string Url,
    string Icon,
    DateTime UploadedAt
);