namespace backend_collab_us.profile_managment.Interfaces.REST.Resources;

public record CommentResource(
    int Id,
    int ProfileId,
    int UserId,
    int Rating,
    string Comment,
    DateTime CreatedAt
);