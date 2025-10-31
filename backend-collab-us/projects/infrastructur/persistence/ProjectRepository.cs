using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.infrastructur.persistence;

public class ProjectRepository(AppDbContext context) : BaseRepository<Project>(context), IProjectRepository
{
    public async Task<IEnumerable<Project>> GetByUserIdAsync(int userId)
    {
        return await Context.Set<Project>()
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByAreaAsync(string area)
    {
        return await Context.Set<Project>()
            .Where(p => p.Areas.Contains(area))
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByTagAsync(string tag)
    {
        return await Context.Set<Project>()
            .Where(p => p.Tags.Contains(tag))
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetBySkillAsync(string skill)
    {
        return await Context.Set<Project>()
            .Where(p => p.Skills.Contains(skill))
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByStatusAsync(string status)
    {
        return await Context.Set<Project>()
            .Where(p => p.Status == status)
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> GetByRoleNameAsync(string roleName)
    {
        return await Context.Set<Project>()
            .Where(p => p.Roles.Any(r => r.Name.Contains(roleName)))
            .ToListAsync();
    }

    public async Task<IEnumerable<Project>> SearchProjectsAsync(
        string? query, 
        string? area, 
        string? roleName,
        string? status = "published")
    {
        var projects = Context.Set<Project>().AsQueryable();

        // Aplicar filtros que pueden ser traducidos a SQL
        if (!string.IsNullOrEmpty(area))
        {
            projects = projects.Where(p => p.Areas.Contains(area));
        }

        if (!string.IsNullOrEmpty(roleName))
        {
            projects = projects.Where(p => p.Roles.Any(r => r.Name.Contains(roleName)));
        }

        if (!string.IsNullOrEmpty(status))
        {
            projects = projects.Where(p => p.Status == status);
        }

        // Ejecutar la consulta inicial para obtener datos de la base de datos
        var result = await projects.ToListAsync();

        // Aplicar filtro de búsqueda en memoria (client-side)
        if (!string.IsNullOrEmpty(query))
        {
            result = result.Where(p => 
                    p.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Summary.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    p.Tags.Any(tag => tag.Contains(query, StringComparison.OrdinalIgnoreCase)) ||
                    p.Skills.Any(skill => skill.Contains(query, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return result;
    }

    public async Task<bool> ExistsByTitleAndUserIdAsync(string title, int userId)
    {
        return await Context.Set<Project>()
            .AnyAsync(p => p.Title == title && p.UserId == userId);
    }

    public async Task<Project?> FindByIdWithRelationsAsync(int projectId)
    {
        return await Context.Set<Project>()
            .Include(p => p.Roles)
            .ThenInclude(r => r.Cards)
            .Include(p => p.AcademicLevelName)
            .Include(p => p.DurationType)
            .Include(p => p.User) // ✅ Esto sigue siendo útil para tener el UserId
            .FirstOrDefaultAsync(p => p.Id == projectId);
    }
    
    public async Task<bool> ExistsByIdAsync(int projectId)
    {
        return await Context.Set<Project>()
            .AnyAsync(p => p.Id == projectId);
    }
    
    public async Task<IEnumerable<Project>> GetAllWithRelationsAsync()
    {
        return await Context.Set<Project>()
            .Include(p => p.Roles)
            .ThenInclude(r => r.Cards)
            .Include(p => p.AcademicLevelName)
            .Include(p => p.DurationType)
            .Include(p => p.User) // Incluir el usuario para obtener el UserId
            .ToListAsync();
    }
}