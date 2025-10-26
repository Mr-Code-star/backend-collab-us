namespace backend_collab_us.IAM.domain.model.commands;
/// <summary>
/// Command of Sign-in
/// </summary>
/// <param name="Email"></param>
/// <param name="Password"></param>
public record SignInCommand(string Email, string Password);