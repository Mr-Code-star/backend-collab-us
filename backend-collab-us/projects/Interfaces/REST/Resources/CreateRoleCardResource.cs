namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record CreateRoleCardResource(
    string Title,
    List<string> Items
);