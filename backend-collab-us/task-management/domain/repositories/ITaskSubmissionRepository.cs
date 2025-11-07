using backend_collab_us.Shared.Domain.Repositories;
using backend_collab_us.task_management.domain.model.agregates;

namespace backend_collab_us.task_management.domain.repositories;

public interface ITaskSubmissionRepository : IBaseRepository<TaskSubmission>
{
    Task<TaskSubmission?> FindByIdAsync(int submissionId);
    Task<IEnumerable<TaskSubmission>> GetByTaskIdAsync(int taskId);
    Task<IEnumerable<TaskSubmission>> GetByCollaboratorIdAsync(int collaboratorId);
    Task<IEnumerable<TaskSubmission>> GetByProjectIdAsync(int projectId);
    Task<IEnumerable<TaskSubmission>> GetPendingReviewAsync(int projectId);
    Task<TaskSubmission?> GetByTaskAndCollaboratorAsync(int taskId, int collaboratorId);
    Task<bool> ExistsByTaskAndCollaboratorAsync(int taskId, int collaboratorId);
}