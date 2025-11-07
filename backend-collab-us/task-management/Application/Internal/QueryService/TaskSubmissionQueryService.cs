using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.queries;
using backend_collab_us.task_management.domain.repositories;
using backend_collab_us.task_management.domain.Services;

namespace backend_collab_us.task_management.Application.Internal.QueryService;

public class TaskSubmissionQueryService : ITaskSubmissionQueryService
{
    private readonly ITaskSubmissionRepository _taskSubmissionRepository;

    public TaskSubmissionQueryService(ITaskSubmissionRepository taskSubmissionRepository)
    {
        _taskSubmissionRepository = taskSubmissionRepository;
    }

    public async Task<TaskSubmission?> Handle(GetTaskSubmissionByIdQuery query)
    {
        return await _taskSubmissionRepository.FindByIdAsync(query.SubmissionId);
    }

    public async Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByTaskIdQuery query)
    {
        return await _taskSubmissionRepository.GetByTaskIdAsync(query.TaskId);
    }

    public async Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByCollaboratorIdQuery query)
    {
        return await _taskSubmissionRepository.GetByCollaboratorIdAsync(query.CollaboratorId);
    }

    public async Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByProjectIdQuery query)
    {
        return await _taskSubmissionRepository.GetByProjectIdAsync(query.ProjectId);
    }

    public async Task<IEnumerable<TaskSubmission>> Handle(GetPendingReviewSubmissionsQuery query)
    {
        return await _taskSubmissionRepository.GetPendingReviewAsync(query.ProjectId);
    }
}