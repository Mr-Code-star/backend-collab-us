using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.valueObjects;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.task_management.Infrastructure.EFC.Configuration.Extentions;

public static class ModelBuilderExtensions
{
    public static void ApplyTasksConfiguration(this ModelBuilder builder)
    {
        // Configuracion de la entidad Task
        builder.Entity<Task>(entity =>
        {
            // Llave primaria
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            // Relacion con Project
            entity.HasOne(t => t.Project)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configuracion de propiedades
            
            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(t => t.Description)
                .HasMaxLength(2000);
            
            entity.Property(t => t.DueDate)
                .IsRequired();
            
            entity.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("pending");
            
            entity.Property(t => t.Priority)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("medium");
            
            entity.Property(t => t.ProjectId)
                .IsRequired();
            
            entity.Property(t => t.AssignedTo)
                .IsRequired();
            
            entity.Property(t => t.AssignedToName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(t => t.Role)
                .HasMaxLength(100);
            
            entity.Property(t => t.Comment)
                .HasMaxLength(1000);
            
            entity.Property(t => t.Progress)
                .IsRequired()
                .HasDefaultValue(0);
            
            entity.Property(t => t.EstimatedHours)
                .IsRequired()
                .HasDefaultValue(0);
            
            entity.Property(t => t.ActualHours)
                .IsRequired()
                .HasDefaultValue(0);
            
            entity.Property(t => t.CreatedBy)
                .IsRequired();
            
            entity.Property(t => t.CreatedAt)
                .IsRequired();
            
            entity.Property(t => t.UpdatedAt)
                .IsRequired();
            
            entity.Property(t => t.CompletedAt)
                .IsRequired(false);
            
            // Indices
            entity.HasIndex(t => t.ProjectId);
            entity.HasIndex(t => t.AssignedTo);
            entity.HasIndex(t => t.Status);
            entity.HasIndex(t => t.Priority);
            entity.HasIndex(t => t.DueDate);
            entity.HasIndex(t => t.CreatedAt);
        });
        // Configuracion de ChecklistItem
        builder.Entity<ChecklistItem>(entity =>
        {
            entity.HasKey(ci => ci.Id);
            entity.Property(ci => ci.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(ci => ci.Text)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(ci => ci.Completed)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(ci => ci.CreatedAt)
                .IsRequired();
            
            // Relación con Task (si decides hacerla entidad separada)
            entity.HasOne<Task>()
                .WithMany(t => t.Checklist)
                .HasForeignKey(ci => ci.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configuracion de TaskTool 
        builder.Entity<TaskTool>(entity =>
        {
            entity.HasKey(tt => tt.Id);
            entity.Property(tt => tt.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(tt => tt.Name)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(tt => tt.Checked)
                .IsRequired()
                .HasDefaultValue(false);
            
            entity.Property(tt => tt.CreatedAt)
                .IsRequired();
            
            entity.HasOne<Task>()
                .WithMany(t => t.Tools)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // Configuracion de TaskAttachment
        builder.Entity<TaskAttachment>(entity =>
        {
            entity.HasKey(ta => ta.Id);
            entity.Property(ta => ta.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(ta => ta.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(ta => ta.Type)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(ta => ta.Url)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(ta => ta.Icon)
                .HasMaxLength(100)
                .HasDefaultValue("pi pi-file");
            
            entity.Property(ta => ta.UploadedAt)
                .IsRequired();
            
            // Relación con Task
            entity.HasOne<Task>()
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // NUEVA CONFIGURACIÓN: TaskSubmission
        ApplyTaskSubmissionConfiguration(builder);
    }
    
    private static void ApplyTaskSubmissionConfiguration(ModelBuilder builder)
    {
        // Configuración de TaskSubmission
        builder.Entity<TaskSubmission>(entity =>
        {
            // Llave primaria
            entity.HasKey(ts => ts.Id);
            entity.Property(ts => ts.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            // Relación con Task
            entity.HasOne(ts => ts.Task)
                .WithMany() // Una tarea puede tener múltiples submissions
                .HasForeignKey(ts => ts.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configuración de propiedades
            entity.Property(ts => ts.TaskId)
                .IsRequired();
            
            entity.Property(ts => ts.CollaboratorId)
                .IsRequired();
            
            entity.Property(ts => ts.CollaboratorName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(ts => ts.SubmittedAt)
                .IsRequired();
            
            entity.Property(ts => ts.Notes)
                .HasMaxLength(2000);
            
            entity.Property(ts => ts.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("submitted");
            
            entity.Property(ts => ts.ReviewNotes)
                .HasMaxLength(2000);
            
            entity.Property(ts => ts.CreatedAt)
                .IsRequired();
            
            entity.Property(ts => ts.UpdatedAt)
                .IsRequired();
            
            // Propiedades opcionales
            entity.Property(ts => ts.ReviewedAt)
                .IsRequired(false);
            
            entity.Property(ts => ts.ReviewerId)
                .IsRequired(false);
            
            // Índices
            entity.HasIndex(ts => ts.TaskId);
            entity.HasIndex(ts => ts.CollaboratorId);
            entity.HasIndex(ts => ts.Status);
            entity.HasIndex(ts => ts.SubmittedAt);
            entity.HasIndex(ts => new { ts.TaskId, ts.CollaboratorId })
                .IsUnique(); // Un colaborador solo puede tener una submission por tarea
        });

        // Configuración de SubmissionLink
        builder.Entity<SubmissionLink>(entity =>
        {
            entity.HasKey(sl => sl.Id);
            entity.Property(sl => sl.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(sl => sl.Url)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(sl => sl.Description)
                .HasMaxLength(500);
            
            entity.Property(sl => sl.CreatedAt)
                .IsRequired();
            
            // Relación con TaskSubmission
            entity.HasOne<TaskSubmission>()
                .WithMany(ts => ts.Links)
                .HasForeignKey(sl => sl.TaskSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de SubmissionAttachment
        builder.Entity<SubmissionAttachment>(entity =>
        {
            entity.HasKey(sa => sa.Id);
            entity.Property(sa => sa.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();
            
            entity.Property(sa => sa.Name)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(sa => sa.Type)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.Property(sa => sa.Url)
                .IsRequired()
                .HasMaxLength(500);
            
            entity.Property(sa => sa.Size)
                .IsRequired();
            
            entity.Property(sa => sa.UploadedAt)
                .IsRequired();
            
            // Relación con TaskSubmission
            entity.HasOne<TaskSubmission>()
                .WithMany(ts => ts.Attachments)
                .HasForeignKey(sa => sa.TaskSubmissionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}