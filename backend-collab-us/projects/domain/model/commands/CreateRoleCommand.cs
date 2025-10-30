namespace backend_collab_us.projects.domain.model.commands;
/// <summary>
/// Command to create a role card within a role
/// </summary>
public record CreateRoleCommand(
    string Name,
    List<CreateRoleCardCommand> Cards
);