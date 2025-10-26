using backend_collab_us.profile_managment.domain.model.queries;
using backend_collab_us.profile_managment.domain.model.agregates;

namespace backend_collab_us.profile_managment.domain.services;

public interface IProfileQueryService
{
    /// <summary>
    /// Handle the GetAllProfilesQuery
    /// </summary>
    /// <remarks>
    /// This method handles the GetAllProfilesQuery. It returns all profiles.
    /// </remarks>
    /// <param name="query">The query object</param>
    /// <returns>A list of all profiles</returns>
    Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query);

    /// <summary>
    /// Handle the GetProfileByIdQuery
    /// </summary>
    /// <remarks>
    /// This method handles the GetProfileByIdQuery. It returns the profile with the specified ID.
    /// </remarks>
    /// <param name="query">The query object containing the ProfileId</param>
    /// <returns>The profile if found, otherwise null</returns>
    Task<Profile?> Handle(GetProfileByIdQuery query);

    
    /// <summary>
    /// Handle the SearchProfilesQuery
    /// </summary>
    /// <remarks>
    /// This method handles the SearchProfilesQuery. It returns profiles that match the given filters.
    /// </remarks>
    /// <param name="query">The query object containing the search filters</param>
    /// <returns>A list of profiles that match the search criteria</returns>
    Task<IEnumerable<Profile>> Handle(SearchProfilesQuery query);
}