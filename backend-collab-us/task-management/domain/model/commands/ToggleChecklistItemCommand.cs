namespace backend_collab_us.task_management.domain.model.commands;

public record ToggleChecklistItemCommand(
    int TaskId,
    int ProjectId,
    int ChecklistItemId
);