namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record ChecklistItemResource(
    int Id,
    string Text,
    bool Completed,
    DateTime CreatedAt,
    DateTime UpdatedAt
);