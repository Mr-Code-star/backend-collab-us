using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.domain.model.commands;
using backend_collab_us.IAM.domain.model.queries;

namespace backend_collab_us.IAM.domain.Services;
/// <summary>
/// interface to service of Query of User
/// </summary>
/// <remarks>
/// This interface defines the basic operations for the favorite sources query service.
/// </remarks>

public interface IUserQueryService
{
    
    Task<User?> Handle(GetUserByIdQuery query);

    /// <summary>
    ///     Handle the GetAllUsersQuery
    /// </summary>
    /// <remarks>
    /// This methods handles the GetAllUsersQuery. It returns all users 
    /// </remarks>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
    
    /// <summary>
    ///  Handle the GetUserByEmailQuery
    /// </summary>
    /// <remarks>
    ///     This methods handles the GetUserByEmailQuery. It returns the user source for the given
    ///     email.
    /// </remarks>
    /// <param name="query"></param>
    /// <returns></returns>
    Task<User?> Handle(GetUserByEmailQuery query);
}