// GetProfileByIdQuery.cs
namespace backend_collab_us.profile_managment.domain.model.queries;

/// <summary>
/// Query to obtain a specific profile by its ID
/// </summary>
public record GetProfileByIdQuery(int ProfileId); // CAMBIAR string por int