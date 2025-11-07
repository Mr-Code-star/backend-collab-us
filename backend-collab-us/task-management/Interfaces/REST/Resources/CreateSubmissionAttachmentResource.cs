namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateSubmissionAttachmentResource(
    string Name,
    string Type,
    string Url,
    long Size
);