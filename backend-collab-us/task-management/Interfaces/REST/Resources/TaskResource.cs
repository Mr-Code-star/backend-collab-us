namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record TaskResource(
    int Id,
    string Title,
    string Description,
    DateTime DueDate,
    string Status,
    string Priority,
    int ProjectId,
    int AssignedTo,
    string AssignedToName,
    string Role,
    string Comment,
    int Progress,
    int EstimatedHours,
    int ActualHours,
    int CreatedBy,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? CompletedAt,
    List<ChecklistItemResource> Checklist,
    List<TaskToolResource> Tools,
    List<TaskAttachmentResource> Attachments
);