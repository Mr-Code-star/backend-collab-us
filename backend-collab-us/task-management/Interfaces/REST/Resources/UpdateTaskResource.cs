namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record UpdateTaskResource(
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