using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.commands;

namespace backend_collab_us.task_management.domain.Services;

public interface ITaskSubmissionCommandService
{
    Task<TaskSubmission?> Handle(CreateTaskSubmissionCommand command);
    Task<TaskSubmission?> Handle(UpdateTaskSubmissionCommand command);
    Task<TaskSubmission?> Handle(ReviewTaskSubmissionCommand command);
    Task<TaskSubmission?> Handle(RequestSubmissionRevisionCommand command);
    Task<TaskSubmission?> Handle(ResubmitTaskSubmissionCommand command);
    Task<bool> Handle(DeleteTaskSubmissionCommand command);
}
