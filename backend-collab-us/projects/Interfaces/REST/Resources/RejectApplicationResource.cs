namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record RejectApplicationResource(
    int ReviewerId,
    string ReviewNotes = ""
);