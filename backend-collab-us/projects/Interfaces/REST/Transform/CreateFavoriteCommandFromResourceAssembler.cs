using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class CreateFavoriteCommandFromResourceAssembler
{
    public static CreateFavoriteCommand ToCommandFromResource(CreateFavoriteResource resource)
    {
        return new CreateFavoriteCommand(
            resource.ProfileId,
            resource.ProjectId
        );
    }
}