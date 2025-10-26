using backend_collab_us.profile_managment.Interfaces.REST.Resources;
using backend_collab_us.profile_managment.domain.model.agregates;

namespace backend_collab_us.profile_managment.Interfaces.REST.Transform;

public static class ProfileResourceFromEntityAssembler
{
    public static ProfileResource ToResourceFromEntity(Profile profile)
    {
        return new ProfileResource(
            profile.Id,
            profile.UserId,
            profile.Username,
            profile.Avatar,
            profile.Role,
            profile.Bio,
            profile.Abilities,
            profile.Experiences,
            profile.Cv,
            profile.Status,
            profile.Points,
            profile.PointsGivenBy ?? new List<string>(),
            profile.CreatedAt,
            profile.UpdatedAt
        );
    }
}