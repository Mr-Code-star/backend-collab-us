namespace backend_collab_us.projects.domain.model.commands;

public record UpdateApplicationStatusCommand(
    int ApplicationId,
    string Status, // "accepted", "rejected", "pending"
    string ReviewNotes = "",
    int ReviewerId = 0
);