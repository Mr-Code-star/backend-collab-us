using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.commands;

namespace backend_collab_us.projects.domain.services;

public interface IProjectCommandService
{
    Task<Project?> Handle(CreateProjectCommand command);
    Task<Project?> Handle(UpdateProjectStatusCommand command); // Podrías necesitar este
}