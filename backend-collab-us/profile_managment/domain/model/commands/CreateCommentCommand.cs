namespace backend_collab_us.profile_managment.domain.model.commands;

public record CreateCommentCommand(
    int ProfileId,
    int UserId,
    int Rating,
    string Comment
);