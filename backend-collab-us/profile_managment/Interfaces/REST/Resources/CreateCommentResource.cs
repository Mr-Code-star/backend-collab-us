namespace backend_collab_us.profile_managment.Interfaces.REST.Resources;

public record CreateCommentResource(
    int ProfileId,
    int UserId,
    int Rating,
    string Comment
);