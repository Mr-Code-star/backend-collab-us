using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;

namespace backend_collab_us.projects.Application.Internal.QueryService;

public class ProjectQueryService(IProjectRepository projectRepository) : IProjectQueryService
{
    public async Task<IEnumerable<Project>> Handle(GetAllProjectsQuery query)
    {
        return await projectRepository.ListAsync();
    }

    public async Task<Project?> Handle(GetProjectByIdQuery query)
    {
        return await projectRepository.FindByIdAsync(query.ProjectId);
    }

    public async Task<IEnumerable<Project>> Handle(GetProjectsByAreaQuery query)
    {
        return await projectRepository.GetByAreaAsync(query.Area);
    }

    public async Task<IEnumerable<Project>> Handle(GetProjectsByTagQuery query)
    {
        return await projectRepository.GetByTagAsync(query.Tag);
    }

    public async Task<IEnumerable<Project>> Handle(GetProjectsBySkillQuery query)
    {
        return await projectRepository.GetBySkillAsync(query.Skill);
    }

    public async Task<IEnumerable<Project>> Handle(GetProjectsByRoleQuery query)
    {
        return await projectRepository.GetByRoleNameAsync(query.RoleName);
    }

    public async Task<IEnumerable<Project>> Handle(SearchProjectsQuery query)
    {
        return await projectRepository.SearchProjectsAsync(
            query.Query,
            query.Area,
            query.RoleName,
            query.status
        );
    }
}