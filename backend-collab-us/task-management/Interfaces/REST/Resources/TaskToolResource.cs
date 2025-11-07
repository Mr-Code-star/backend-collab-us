namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record TaskToolResource(
    int Id,
    string Name,
    bool Checked,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
