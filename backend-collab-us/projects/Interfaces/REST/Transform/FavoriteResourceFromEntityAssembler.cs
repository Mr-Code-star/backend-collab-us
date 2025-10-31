using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class FavoriteResourceFromEntityAssembler
{
    public static FavoriteResource ToResourceFromEntity(Favorite favorite)
    {
        return new FavoriteResource(
            favorite.Id,
            favorite.ProfileId,
            favorite.ProjectId,
            favorite.CreatedAt
        );
    }
}