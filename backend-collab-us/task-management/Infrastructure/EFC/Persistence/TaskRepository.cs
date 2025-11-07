using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using backend_collab_us.task_management.domain.repositories;
using Microsoft.EntityFrameworkCore;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
namespace backend_collab_us.task_management.Infrastructure.EFC.Persistence;

public class TaskRepository(AppDbContext context) : BaseRepository<Task>(context), ITaskRepository
{
    public async Task<IEnumerable<Task>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Set<Task>()
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .Include(t => t.Checklist)        // ✅ INCLUIR CHECKLIST
            .Include(t => t.Tools)            // ✅ INCLUIR HERRAMIENTAS
            .Include(t => t.Attachments)      // ✅ INCLUIR ARCHIVOS ADJUNTOS
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetByAssignedToNameAsync(int projectId, string assignedToName)
    {
        return await Context.Set<Task>()
            .Where(t => t.ProjectId == projectId && 
                        t.AssignedToName.Contains(assignedToName))
            .ToListAsync();
    }

    public async Task<IEnumerable<Task>> SearchTasksAsync(int projectId, string? assignedToName = null, string? status = null)
    {
        var query = Context.Set<Task>()
            .Where(t => t.ProjectId == projectId)
            .Include(t => t.Project)
            .Include("_checklist")    // ✅ Usar nombres de campos privados
            .Include("_tools")
            .Include("_attachments");

        if (!string.IsNullOrEmpty(assignedToName))
        {
            query = query.Where(t => t.AssignedToName.Contains(assignedToName));
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(t => t.Status == status);
        }
    
        return await query.ToListAsync();
    }

    public async Task<IEnumerable<Task>> GetOverdueTasksAsync(int projectId)
    {
        var now = DateTime.Now;
        return await Context.Set<Task>()
            .Where(t => t.ProjectId == projectId && 
                        t.Status != "completed" && 
                        t.DueDate < now)
            .ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(int taskId)
    {
        return await Context.Set<Task>()
            .AnyAsync(t => t.Id == taskId);
    }

    public async Task<Task?> FindByIdWithRelationsAsync(int taskId)
    {
        return await Context.Set<Task>()
            .Include(t => t.Project)
            .Include(t => t.Checklist)        // ✅ INCLUIR CHECKLIST
            .Include(t => t.Tools)            // ✅ INCLUIR HERRAMIENTAS
            .Include(t => t.Attachments)      // ✅ INCLUIR ARCHIVOS ADJUNTOS
            .FirstOrDefaultAsync(t => t.Id == taskId);
    }

    public async Task<IEnumerable<Task>> GetUpcomingDueTasksAsync(int projectId, int daysAhead = 3)
    {
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(daysAhead);

        return await Context.Set<Task>()
            .Where(t => t.ProjectId == projectId && 
                        t.Status != "completed" &&
                        t.DueDate >= startDate && 
                        t.DueDate <= endDate)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }
}