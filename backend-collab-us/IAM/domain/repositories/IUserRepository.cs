using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.Shared.Domain.Repositories;

namespace backend_collab_us.IAM.domain.repositories;
/// <summary>
///  The contract of repository of User
/// </summary>
/// <remarks>
///  This interface define the operation CRUD to repository of User
///  inherits all the operations basics CRUD of IRepositoryBase and add methods specialized
/// </remarks>
public interface IUserRepository : IBaseRepository<User>
{
    // Method of Find User by Email
    Task<User?>  FindByEmail(string email);
    
    Task<bool> ExistsByIdAsync(int userId);
}