using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.queries;

namespace backend_collab_us.task_management.domain.Services;

public interface ITaskSubmissionQueryService
{
    Task<TaskSubmission?> Handle(GetTaskSubmissionByIdQuery query);
    Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByTaskIdQuery query);
    Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByCollaboratorIdQuery query);
    Task<IEnumerable<TaskSubmission>> Handle(GetSubmissionsByProjectIdQuery query);
    Task<IEnumerable<TaskSubmission>> Handle(GetPendingReviewSubmissionsQuery query);
}