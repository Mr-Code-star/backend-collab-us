namespace backend_collab_us.task_management.domain.model.commands;

public record ReviewTaskSubmissionCommand(
    int SubmissionId,
    int ReviewerId,
    string ReviewNotes
);