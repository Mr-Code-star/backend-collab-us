namespace backend_collab_us.task_management.domain.model.commands;

public record UpdateTaskCommand(
    int TaskId,
    int ProjectId,
    string Title,
    string Description,
    DateTime DueDate,
    string Status,
    string Priority,
    int AssignedTo,
    string AssignedToName,
    string Role,
    string Comment,
    int EstimatedHours,
    int ActualHours
);