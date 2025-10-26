using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.Interfaces.REST.Resources;

namespace backend_collab_us.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static UserResource ToResourceFromEntity(User user)
    {
        return new UserResource(
            user.Id,
            user.FullName,
            user.Email,
            user.Status,
            user.Password,
            user.CreatedAt,
            user.UpdatedAt
        );
    }
}