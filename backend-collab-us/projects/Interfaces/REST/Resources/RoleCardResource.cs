namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record RoleCardResource(
    int Id,
    string Title,
    List<string> Items,
    DateTime CreatedAt,
    DateTime UpdatedAt
);