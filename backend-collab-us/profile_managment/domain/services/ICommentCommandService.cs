using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.model.commands;

namespace backend_collab_us.profile_managment.domain.services;

public interface ICommentCommandService
{
    Task<Comment?> Handle(CreateCommentCommand command);
}