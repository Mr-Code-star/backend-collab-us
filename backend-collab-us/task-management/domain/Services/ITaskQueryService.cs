using backend_collab_us.task_management.domain.model.queries;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
namespace backend_collab_us.task_management.domain.Services;

public interface ITaskQueryService
{
    // Queries basicos de tareas
    Task<IEnumerable<Task>> Handle(GetProjectTasksQuery query);
    Task<IEnumerable<Task>> Handle(SearchTasksQuery query);

}