using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.profile_managment.Infrastructure.Persistence.Repositories;


public class CommentRepository : BaseRepository<Comment>, ICommentRepository
{
    public CommentRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> FindByProfileIdAsync(int profileId)
    {
        return await Context.Set<Comment>()
            .Where(c => c.ProfileId == profileId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync();
    }
}