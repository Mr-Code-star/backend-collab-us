namespace backend_collab_us.task_management.Interfaces.REST.Resources;

public record ReviewTaskSubmissionResource(
    int ReviewerId,
    string ReviewNotes
);