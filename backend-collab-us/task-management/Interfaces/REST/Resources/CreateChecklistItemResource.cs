namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateChecklistItemResource(
    string Text,
    bool Completed = false
);