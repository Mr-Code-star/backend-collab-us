namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateTaskAttachmentResource(
    string Name,
    string Type,
    string Url,
    string? Icon = null
);