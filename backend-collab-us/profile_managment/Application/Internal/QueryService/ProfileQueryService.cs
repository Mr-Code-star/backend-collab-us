using backend_collab_us.profile_managment.domain.model.queries;
using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.profile_managment.domain.services;

namespace backend_collab_us.profile_managment.Application.Internal.QueryService;

public class ProfileQueryService(IProfileRepository profileRepository) : IProfileQueryService
{
    public async Task<IEnumerable<Profile>> Handle(GetAllProfilesQuery query)
    {
        return await profileRepository.ListAsync();
    }

    public async Task<Profile?> Handle(GetProfileByIdQuery query)
    {
        return await profileRepository.FindByIdAsync(query.ProfileId); // QUITAR "String" del nombre
    }
    public async Task<IEnumerable<Profile>> Handle(SearchProfilesQuery query)
    {
        return await profileRepository.SearchProfilesAsync(
            query.Query,
            query.Role,
            query.MinScore,
            query.MaxScore
        );    
    }
}