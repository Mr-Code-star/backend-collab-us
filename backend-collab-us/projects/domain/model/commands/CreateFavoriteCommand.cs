namespace backend_collab_us.projects.domain.model.commands;

/// <summary>
/// Command to create a favorite project
/// </summary>
public record CreateFavoriteCommand(
    int ProfileId,
    int ProjectId
);