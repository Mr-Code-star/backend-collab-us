namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record CreateRoleResource(
    string Name,
    List<CreateRoleCardResource> Cards
);