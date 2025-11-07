namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateTaskToolResource(
    string Name,
    bool Checked = false
);