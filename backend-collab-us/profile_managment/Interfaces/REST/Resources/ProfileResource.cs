using backend_collab_us.profile_managment.domain.model.valueObjects;

namespace backend_collab_us.profile_managment.Interfaces.REST.Resources;

public record ProfileResource(
    int Id, // CAMBIADO de string a int
    int UserId,
    string Username,
    string? Avatar,
    string Role,
    string Bio,
    List<string> Abilities,
    List<Experience> Experiences,
    CV? Cv,
    string Status,
    int Points,
    DateTime CreatedAt,
    DateTime UpdatedAt
);