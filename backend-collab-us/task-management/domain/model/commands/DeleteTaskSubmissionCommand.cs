namespace backend_collab_us.task_management.domain.model.commands;

public record DeleteTaskSubmissionCommand(
    int SubmissionId,
    int DeletedBy
);