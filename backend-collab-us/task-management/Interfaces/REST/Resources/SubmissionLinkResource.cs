namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record SubmissionLinkResource(
    int Id,
    string Url,
    string Description,
    DateTime CreatedAt
);