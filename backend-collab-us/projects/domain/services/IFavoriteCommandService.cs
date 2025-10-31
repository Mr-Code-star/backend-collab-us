using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.services;

public interface IFavoriteCommandService
{
    Task<Favorite?> Handle(CreateFavoriteCommand command);

}