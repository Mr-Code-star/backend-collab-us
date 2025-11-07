using backend_collab_us.task_management.domain.model.queries;
using backend_collab_us.task_management.domain.repositories;
using backend_collab_us.task_management.domain.Services;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
namespace backend_collab_us.task_management.Application.Internal.QueryService;

public class TaskQueryService(ITaskRepository taskRepository) : ITaskQueryService
{
    public async Task<IEnumerable<Task?>> Handle(GetProjectTasksQuery query)
    {
        return await taskRepository.GetByProjectIdAsync(query.ProjectId);
    }

    public async Task<IEnumerable<Task?>> Handle(SearchTasksQuery query)
    {
        return await taskRepository.SearchTasksAsync(
            query.ProjectId,
            query.AssignedToName,
            query.Status
        );
    }
}