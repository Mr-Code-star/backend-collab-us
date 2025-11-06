namespace backend_collab_us.task_management.domain.model.commands;

public record RemoveTaskAttachmentCommand(
    int TaskId,
    int ProjectId,
    int AttachmentId
);