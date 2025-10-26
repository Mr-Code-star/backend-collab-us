using backend_collab_us.IAM.domain.model.commands;
using backend_collab_us.IAM.Interfaces.REST.Resources;

namespace backend_collab_us.IAM.Interfaces.REST.Transform;

public static class CreateUserCommandFromResourceAssembler
{
    public static CreateUserCommand ToCommandFromResource(CreateUserResource resource)
    {
        return new CreateUserCommand(
            resource.FullName,
            resource.Email,
            resource.Password
        );
    }
}