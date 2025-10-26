using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.IAM.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context): BaseRepository<User>(context), IUserRepository
{
    public new async Task<User?> FindByIdAsync(int id)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public new async Task<IEnumerable<User>> ListAsync()
    {
        return await Context.Set<User>()
            .ToListAsync();
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await Context.Set<User>()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByIdAsync(int userId)
    {
        return await Context.Set<User>()
            .AnyAsync(u => u.Id == userId);
    }
}