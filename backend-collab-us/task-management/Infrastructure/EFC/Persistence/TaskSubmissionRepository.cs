using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.valueObjects;
using backend_collab_us.task_management.domain.repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.task_management.Infrastructure.EFC.Persistence;

public class TaskSubmissionRepository : BaseRepository<TaskSubmission>, ITaskSubmissionRepository
{
    public TaskSubmissionRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<TaskSubmission?> FindByIdAsync(int submissionId)
    {
        return await Context.Set<TaskSubmission>()
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .FirstOrDefaultAsync(ts => ts.Id == submissionId);
    }

    public async Task<IEnumerable<TaskSubmission>> GetByTaskIdAsync(int taskId)
    {
        return await Context.Set<TaskSubmission>()
            .Where(ts => ts.TaskId == taskId)
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskSubmission>> GetByCollaboratorIdAsync(int collaboratorId)
    {
        return await Context.Set<TaskSubmission>()
            .Where(ts => ts.CollaboratorId == collaboratorId)
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskSubmission>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Set<TaskSubmission>()
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .Where(ts => ts.Task.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskSubmission>> GetPendingReviewAsync(int projectId)
    {
        return await Context.Set<TaskSubmission>()
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .Where(ts => ts.Task.ProjectId == projectId && ts.Status == SubmissionStatus.SUBMITTED)
            .ToListAsync();
    }

    public async Task<TaskSubmission?> GetByTaskAndCollaboratorAsync(int taskId, int collaboratorId)
    {
        return await Context.Set<TaskSubmission>()
            .Include(ts => ts.Task)
            .Include(ts => ts.Links)
            .Include(ts => ts.Attachments)
            .FirstOrDefaultAsync(ts => ts.TaskId == taskId && ts.CollaboratorId == collaboratorId);
    }

    public async Task<bool> ExistsByTaskAndCollaboratorAsync(int taskId, int collaboratorId)
    {
        return await Context.Set<TaskSubmission>()
            .AnyAsync(ts => ts.TaskId == taskId && ts.CollaboratorId == collaboratorId);
    }
}