namespace backend_collab_us.task_management.domain.model.commands;

public record CreateTaskToolCommand(
    string Name,
    bool Checked = false
);