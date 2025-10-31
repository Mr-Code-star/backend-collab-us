namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record UpdateApplicationStatusResource(
    string Status,
    string ReviewNotes = "",
    int ReviewerId = 0
);