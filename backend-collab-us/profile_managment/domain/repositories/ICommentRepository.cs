using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.profile_managment.domain.repositories;

public interface ICommentRepository : IBaseRepository<Comment>
{
    Task<IEnumerable<Comment>> FindByProfileIdAsync(int profileId);
}