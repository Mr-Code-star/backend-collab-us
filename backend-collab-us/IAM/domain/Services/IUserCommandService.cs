using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.domain.model.commands;

namespace backend_collab_us.IAM.domain.Services;

/// <summary>
///  Service of Commands for Users
/// </summary>
/// <remarks>
/// </remarks>
public interface IUserCommandService
{
    /// <summary>
    /// Handle of the creates User
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<User?> Handle(CreateUserCommand command);
    
    /// <summary>
    /// Handle of the sign-in
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<(User user, string token)> Handle(SignInCommand command);

}