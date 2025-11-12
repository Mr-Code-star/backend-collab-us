using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.Infrastructure.Persistence.EFC.Configuration.Extentions;
using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.profile_managment.Infrastructure.Persistence.EFC.Configuration.Extentions;
using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.valueObjects;
using backend_collab_us.projects.infrastructur.EFC.Configuration.Extentions;
using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.valueObjects;
using backend_collab_us.task_management.Infrastructure.EFC.Configuration.Extentions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;

namespace backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // IAM Bounded Context
    public DbSet<User> Users { get; set; }
    
    // Profile Management Bounded Context
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    
    // Projects Bounded Context 
    public DbSet<Project> Projects { get; set; }
    public DbSet<Application> Applications { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Rol> Roles { get; set; }
    public DbSet<RoleCard> RoleCards { get; set; }
    public DbSet<AcademicLevel> AcademicLevels { get; set; }
    public DbSet<DurationType> DurationTypes { get; set; }
    public DbSet<Area> Areas { get; set; }
    
    // Task Management Bounded Context
    public DbSet<Task> Tasks { get; set; }
    public DbSet<ChecklistItem> ChecklistItems { get; set; }
    public DbSet<TaskTool> TaskTools { get; set; }
    public DbSet<TaskAttachment> TaskAttachments { get; set; }

    // Task Submission Bounded Context
    public DbSet<TaskSubmission> TaskSubmissions { get; set; }
    public DbSet<SubmissionLink> SubmissionLinks { get; set; }
    public DbSet<SubmissionAttachment> SubmissionAttachments { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // ✅ SOLUCIÓN: FORZAR NOMBRES DE TABLAS EN MINÚSCULA
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            // Cambiar nombre de tabla a minúscula
            var currentTableName = entity.GetTableName();
            if (!string.IsNullOrEmpty(currentTableName))
            {
                entity.SetTableName(currentTableName.ToLower());
            }
            
            // Cambiar nombres de columnas a minúscula también
            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(property.Name.ToLower());
            }
            
            // Cambiar nombres de claves foráneas a minúscula
            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName().ToLower());
            }
            
            // Cambiar nombres de índices a minúscula
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName().ToLower());
            }
        }
        
        // Apply configurations for both bounded contexts
        builder.ApplyIamConfiguration();
        builder.ApplyProfileManagementConfiguration();
        builder.ApplyProjectsConfiguration();
        builder.ApplyTasksConfiguration();
    }
}