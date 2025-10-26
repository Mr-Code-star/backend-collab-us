using backend_collab_us.IAM.domain.model.commands;
using backend_collab_us.IAM.Interfaces.REST.Resources;

namespace backend_collab_us.IAM.Interfaces.REST.Transform;

public static class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource  resource)
    {
        return new SignInCommand(resource.Email, resource.Password);
    }
}