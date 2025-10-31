namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record AcceptApplicationResource(
    int ReviewerId,
    string ReviewNotes = ""
);