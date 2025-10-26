namespace backend_collab_us.profile_managment.domain.model.commands;

public record UpdateProfilePointsCommand(
    int ProfileId,
    int Points,
    List<string> PointsGivenBy
);