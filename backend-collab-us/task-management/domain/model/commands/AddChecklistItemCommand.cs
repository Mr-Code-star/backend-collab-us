namespace backend_collab_us.task_management.domain.model.commands;

public record AddChecklistItemCommand(
    int TaskId,
    int ProjectId,
    string Text
);