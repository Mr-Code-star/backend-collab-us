// SearchProfilesQuery.cs
namespace backend_collab_us.profile_managment.domain.model.queries;

/// <summary>
/// Query to search profiles with filters
/// </summary>
public record SearchProfilesQuery(
    string? Query,
    string? Role,
    int? MinScore,
    int? MaxScore,
    bool ExcludeCurrentUser = true
);