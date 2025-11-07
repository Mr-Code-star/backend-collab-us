namespace backend_collab_us.task_management.domain.model.commands;

public record RequestSubmissionRevisionCommand(
    int SubmissionId,
    string RevisionNotes
);