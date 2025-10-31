namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record RoleResource(
    int Id,
    string Name,
    List<RoleCardResource> Cards,
    DateTime CreatedAt,
    DateTime UpdatedAt
);