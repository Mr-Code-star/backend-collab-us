using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.queries;

namespace backend_collab_us.projects.domain.services;

public interface IProjectQueryService
{
    Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query);
    Task<Project?> Handle(GetProjectByIdQuery query);
    Task<IEnumerable<Project>> Handle(GetProjectsByAreaQuery query);
    Task<IEnumerable<Project>> Handle(GetProjectsByTagQuery query);
    Task<IEnumerable<Project>> Handle(GetProjectsBySkillQuery query);
    Task<IEnumerable<Project>> Handle(GetProjectsByRoleQuery query); // Nuevo, como discutimos
    Task<IEnumerable<Project>> Handle(SearchProjectsQuery query); // Podrías necesitar este
}