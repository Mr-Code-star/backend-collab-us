namespace backend_collab_us.task_management.domain.model.commands;

public record CreateChecklistItemCommand(
    string Text,
    bool Completed = false
);