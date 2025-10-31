using backend_collab_us.projects.domain.repositories;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.infrastructur.persistence;

public class ApplicationRepository(AppDbContext context) : BaseRepository<domain.model.agregates.Application>(context), IApplicationRepository
{
    public async Task<IEnumerable<domain.model.agregates.Application>> GetByProjectIdAsync(int projectId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .Where(a => a.ProjectId == projectId)
            .ToListAsync();
    }

    public async Task<IEnumerable<domain.model.agregates.Application>> GetByApplicantIdAsync(int applicantId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .Where(a => a.ApplicantId == applicantId)
            .ToListAsync();
    }

    public async Task<IEnumerable<domain.model.agregates.Application>> GetByRoleIdAsync(long roleId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .Where(a => a.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<IEnumerable<domain.model.agregates.Application>> GetByStatusAsync(string status)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .Where(a => a.Status == status)
            .ToListAsync();
    }

    public async Task<bool> ExistsByProjectAndApplicantAsync(int projectId, int applicantId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .AnyAsync(a => a.ProjectId == projectId && a.ApplicantId == applicantId);
    }

    public async Task<bool> ExistsByProjectApplicantAndRoleAsync(int projectId, int applicantId, long roleId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .AnyAsync(a => a.ProjectId == projectId && a.ApplicantId == applicantId && a.RoleId == roleId);
    }

    public async Task<int> CountByProjectIdAsync(int projectId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .CountAsync(a => a.ProjectId == projectId);
    }

    public async Task<int> CountByApplicantIdAsync(int applicantId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .CountAsync(a => a.ApplicantId == applicantId);
    }

    public async Task<domain.model.agregates.Application?> FindByIdWithRelationsAsync(int applicationId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .Include(a => a.Project)
            .Include(a => a.Applicant)
            .FirstOrDefaultAsync(a => a.Id == applicationId);
    }
    
    public async Task<bool> ExistsByIdAsync(int applicationId)
    {
        return await Context.Set<domain.model.agregates.Application>()
            .AnyAsync(a => a.Id == applicationId);
    }
}