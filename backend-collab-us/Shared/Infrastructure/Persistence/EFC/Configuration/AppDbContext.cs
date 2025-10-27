using backend_collab_us.comment_managment.domain.model.agregates;
using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.Infrastructure.Persistence.EFC.Configuration.Extentions;
using backend_collab_us.profile_managment.domain.model.agregates;
using backend_collab_us.profile_managment.Infrastructure.Persistence.EFC.Configuration.Extentions;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // IAM Bounded Context
    public DbSet<User> Users { get; set; }
    
    // Profile Management Bounded Context
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Comment> Comments { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddCreatedUpdatedInterceptor();
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply configurations for both bounded contexts
        builder.ApplyIamConfiguration();
        builder.ApplyProfileManagementConfiguration();
    }
}