using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    /// <inheritdoc />

    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}