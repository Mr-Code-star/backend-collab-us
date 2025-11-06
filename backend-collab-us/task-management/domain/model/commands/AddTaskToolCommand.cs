namespace backend_collab_us.task_management.domain.model.commands;

public record AddTaskToolCommand(
    int TaskId,
    int ProjectId,
    string Name
);