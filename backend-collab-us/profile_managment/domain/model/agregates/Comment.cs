using backend_collab_us.profile_managment.domain.model.commands;

namespace backend_collab_us.comment_managment.domain.model.agregates;

public partial class Comment
{
    public int Id { get; private set; }
    public int ProfileId { get; private set; } // Profile being commented
    public int UserId { get; private set; } // User who commented
    public int Rating { get; private set; }
    public string CommentText { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Comment()
    {
        CommentText = string.Empty;
        CreatedAt = DateTime.UtcNow;
    }

    // Constructor principal
    public Comment(int profileId, int userId, int rating, string commentText)
    {
        ProfileId = profileId;
        UserId = userId;
        Rating = rating;
        CommentText = commentText;
        CreatedAt = DateTime.UtcNow;
    }

    // Constructor desde comando
    public Comment(CreateCommentCommand command)
        : this(command.ProfileId, command.UserId, command.Rating, command.Comment)
    {
    }

    // Método para actualizar comentario
    public void UpdateComment(int rating, string commentText)
    {
        Rating = rating;
        CommentText = commentText;
    }
}