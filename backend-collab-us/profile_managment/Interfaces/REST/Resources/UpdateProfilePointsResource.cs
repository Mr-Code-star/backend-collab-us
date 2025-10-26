namespace backend_collab_us.profile_managment.Interfaces.REST.Resources;

public record UpdateProfilePointsResource(
    int Points,
    List<string> PointsGivenBy
);

