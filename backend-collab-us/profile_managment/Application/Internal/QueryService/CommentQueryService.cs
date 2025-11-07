using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.model.queries;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.profile_managment.domain.services;

namespace backend_collab_us.comment_managment.Application.Internal.QueryService;

public class CommentQueryService(ICommentRepository commentRepository) : ICommentQueryService
{
    public async Task<IEnumerable<Comment>> Handle(GetCommentsByProfileIdQuery query)
    {
        var comments = await commentRepository.FindByProfileIdAsync(query.ProfileId);
        return comments ?? Enumerable.Empty<Comment>(); // ✅ Asegurar que nunca sea null
    }
}