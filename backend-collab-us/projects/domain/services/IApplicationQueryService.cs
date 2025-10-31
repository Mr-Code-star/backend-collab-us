using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.queries;

namespace backend_collab_us.projects.domain.services;

public interface IApplicationQueryService
{
    Task<IEnumerable<model.agregates.Application>> Handle(GetApplicationsByProjectIdQuery query);
    Task<model.agregates.Application?> Handle(GetApplicationByIdQuery query);
    Task<model.agregates.Application?> Handle(GetApplicationDetailsQuery query);
}