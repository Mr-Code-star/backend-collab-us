using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.Interfaces.REST.Resources;

namespace backend_collab_us.IAM.Interfaces.REST.Transform;

public class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(user.Id, user.FullName, token);
    }
}