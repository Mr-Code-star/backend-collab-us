namespace backend_collab_us.task_management.domain.model.commands;

public record CreateTaskCommand(
    string Title,
    string Description,
    DateTime DueDate,
    string Status,
    string Priority,
    int ProjectId,
    int AssignedTo,
    string AssignedToName,
    string Role,
    List<CreateChecklistItemCommand> Checklist,
    List<CreateTaskToolCommand> Tools,
    string Comment,
    List<CreateTaskAttachmentCommand> Attachments,
    int EstimatedHours,
    int CreatedBy
);