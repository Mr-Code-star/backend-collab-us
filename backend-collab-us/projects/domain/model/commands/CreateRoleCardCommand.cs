namespace backend_collab_us.projects.domain.model.commands;
/// <summary>
/// Command to create a role within a project
/// </summary>
public record CreateRoleCardCommand(
    string Title,
    List<string> Items
);