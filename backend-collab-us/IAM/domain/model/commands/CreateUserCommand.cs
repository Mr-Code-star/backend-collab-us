namespace backend_collab_us.IAM.domain.model.commands;

/// <summary>
/// Comand to creation a user
/// the comands always be record 
/// </summary>
public record CreateUserCommand(
    string FullName,
    string Email,
    string Password
);