using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.queries;

namespace backend_collab_us.projects.domain.services;

public interface IFavoriteQueryService
{
    Task<IEnumerable<Favorite>> Handle(GetFavoritesByProfileIdQuery query);
    Task<IEnumerable<Project>> Handle(GetFavoriteProjectsByProfileIdQuery query);
    Task<bool> Handle(CheckIfProjectIsFavoriteQuery query);
    
}