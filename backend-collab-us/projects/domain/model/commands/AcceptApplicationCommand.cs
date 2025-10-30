namespace backend_collab_us.projects.domain.model.commands;

public record AcceptApplicationCommand(
    int ApplicationId,
    int ReviewerId,
    string ReviewNotes = ""
);