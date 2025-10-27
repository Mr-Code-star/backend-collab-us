using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.Interfaces.REST.Resources;

namespace backend_collab_us.profile_managment.Interfaces.REST.Transform;

public class CreateCommentCommandFromResourceAssembler
{
    public static CreateCommentCommand ToCommandFromResource(CreateCommentResource resource)
    {
        return new CreateCommentCommand(
            resource.ProfileId,
            resource.UserId,
            resource.Rating,
            resource.Comment
        );
    }
}