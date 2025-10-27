using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.profile_managment.Interfaces.REST.Resources;

namespace backend_collab_us.profile_managment.Interfaces.REST.Transform;

public static class CommentResourceFromEntityAssembler
{
    public static CommentResource ToResourceFromEntity(Comment comment)
    {
        return new CommentResource(
            comment.Id,
            comment.ProfileId,
            comment.UserId,
            comment.Rating,
            comment.CommentText,
            comment.CreatedAt
        );
    }
}