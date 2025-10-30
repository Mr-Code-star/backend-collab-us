namespace backend_collab_us.projects.domain.model.commands;

public record RejectApplicationCommand(
    int ApplicationId,
    int ReviewerId,
    string ReviewNotes = ""
);