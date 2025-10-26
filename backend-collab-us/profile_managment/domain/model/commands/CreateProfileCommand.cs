using backend_collab_us.profile_managment.domain.model.valueObjects;

namespace backend_collab_us.profile_managment.domain.model.commands;

public record CreateProfileCommand(
    int UserId,
    string Username,
    string? Avatar,
    string Role,
    string Bio,
    List<string> Abilities,
    List<Experience> Experiences,
    CV? Cv
);