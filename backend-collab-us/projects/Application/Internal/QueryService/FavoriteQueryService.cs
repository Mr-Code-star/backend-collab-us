using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;

namespace backend_collab_us.projects.Application.Internal.QueryService;

public class FavoriteQueryService(
    IFavoriteRepository favoriteRepository,
    IProjectRepository projectRepository) : IFavoriteQueryService
{
    public async Task<IEnumerable<Favorite>> Handle(GetFavoritesByProfileIdQuery query)
    {
        return await favoriteRepository.GetByProfileIdAsync(query.ProfileId);
    }

    public async Task<IEnumerable<Project>> Handle(GetFavoriteProjectsByProfileIdQuery query)
    {
        var favorites = await favoriteRepository.GetByProfileIdAsync(query.ProfileId);
        var projectIds = favorites.Select(f => f.ProjectId).Distinct();
        
        var projects = new List<Project>();
        foreach (var projectId in projectIds)
        {
            var project = await projectRepository.FindByIdAsync(projectId);
            if (project != null) projects.Add(project);
        }
        
        return projects;
    }

    public async Task<bool> Handle(CheckIfProjectIsFavoriteQuery query)
    {
        return await favoriteRepository.ExistsByProfileAndProjectAsync(query.ProfileId, query.ProjectId);
    }
}