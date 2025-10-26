namespace backend_collab_us.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();

}