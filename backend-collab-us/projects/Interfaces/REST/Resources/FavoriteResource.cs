namespace backend_collab_us.projects.Interfaces.REST.Resources;

public record FavoriteResource(
    int Id,
    int ProfileId,
    int ProjectId,
    DateTime CreatedAt
);