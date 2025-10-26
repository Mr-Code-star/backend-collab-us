namespace backend_collab_us.IAM.Interfaces.REST.Resources;

public record UserResource(
    int Id,
    string FullName,
    string Email,
    string Status,
    string Password,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );