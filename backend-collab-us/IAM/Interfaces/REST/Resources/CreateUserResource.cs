namespace backend_collab_us.IAM.Interfaces.REST.Resources;

public record CreateUserResource(
    string FullName,
    string Email,
    string Password
);