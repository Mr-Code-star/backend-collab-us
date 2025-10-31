namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record CreateFavoriteResource(
    int ProfileId,
    int ProjectId
);