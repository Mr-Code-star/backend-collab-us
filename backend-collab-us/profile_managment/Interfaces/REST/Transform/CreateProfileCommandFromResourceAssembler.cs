using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.Interfaces.REST.Resources;

namespace backend_collab_us.profile_managment.Interfaces.REST.Transform;

public class CreateProfileCommandFromResourceAssembler
{
    public static CreateProfileCommand ToCommandFromResource(CreateProfileResource resource)
    {
        return new CreateProfileCommand(
            resource.UserId,
            resource.Username,
            resource.Avatar,
            resource.Role,
            resource.Bio,
            resource.Abilities,
            resource.Experiences,
            resource.Cv
        );
    }
}

