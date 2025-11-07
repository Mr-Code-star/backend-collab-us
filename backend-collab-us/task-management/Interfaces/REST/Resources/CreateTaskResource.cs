namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record CreateTaskResource(
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
    List<CreateChecklistItemResource> Checklist,
    List<CreateTaskToolResource> Tools,
    List<CreateTaskAttachmentResource> Attachments,
    int EstimatedHours,
    int CreatedBy
);