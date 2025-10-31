using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.repositories;
using backend_collab_us.projects.domain.services;

namespace backend_collab_us.projects.Application.Internal.QueryService;

public class ApplicationQueryService(IApplicationRepository applicationRepository) : IApplicationQueryService
{
    public async Task<IEnumerable<domain.model.agregates.Application>> Handle(GetApplicationsByProjectIdQuery query)
    {
        return await applicationRepository.GetByProjectIdAsync(query.ProjectId);
    }
    
    public async Task<domain.model.agregates.Application?> Handle(GetApplicationByIdQuery query)
    {
        return await applicationRepository.FindByIdAsync(query.ApplicationId);
    }

    public async Task<domain.model.agregates.Application?> Handle(GetApplicationDetailsQuery query)
    {
        return await applicationRepository.FindByIdWithRelationsAsync(query.ApplicationId);
    }
}