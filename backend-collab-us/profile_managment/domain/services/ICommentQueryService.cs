using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.model.queries;

namespace backend_collab_us.profile_managment.domain.services;

public interface ICommentQueryService
{
    Task<IEnumerable<Comment>> Handle(GetCommentsByProfileIdQuery query);

}