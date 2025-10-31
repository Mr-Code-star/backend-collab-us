using System.Text.Json;
using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.domain.model.valueObjects;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.infrastructur.EFC.Configuration.Extentions;

public static class ModelBuilderExtensions
{
    public static void ApplyProjectsConfiguration(this ModelBuilder builder)
    {
        // Configuración de la entidad Project
        builder.Entity<Project>(entity =>
        {
            // Llave primaria
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Relación con User
            entity.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con AcademicLevel
            entity.HasOne(p => p.AcademicLevelName)
                .WithMany()
                .HasForeignKey("AcademicLevelId") // Nombre de la FK en la base de datos
                .OnDelete(DeleteBehavior.Restrict);

            // Relación con DurationType
            entity.HasOne(p => p.DurationType)
                .WithMany()
                .HasForeignKey("DurationTypeId") // Nombre de la FK en la base de datos
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades
            entity.Property(p => p.UserId)
                .IsRequired();

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(p => p.Summary)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(p => p.Benefits)
                .HasMaxLength(1000);

            entity.Property(p => p.Skills)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");

            entity.Property(p => p.DurationQuantity)
                .IsRequired();

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("draft");

            entity.Property(p => p.Progress)
                .IsRequired()
                .HasDefaultValue(0);

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.UpdatedAt)
                .IsRequired();

            // Configuración de colecciones (para EF Core 5+)
            entity.Property(p => p._areas)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text")
                .HasColumnName("Areas");

            entity.Property(p => p._tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text")
                .HasColumnName("Tags");

            // Índices
            entity.HasIndex(p => p.UserId);
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => p.CreatedAt);
            entity.HasIndex(p => p.Title);
        });

        // Configuración de la entidad Application
        builder.Entity<domain.model.agregates.Application>(entity =>
        {
            // Llave primaria
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Relación con Project
            entity.HasOne(a => a.Project)
                .WithMany(p => p.Collaborators)
                .HasForeignKey(a => a.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación con User (Applicant)
            entity.HasOne(a => a.Applicant)
                .WithMany()
                .HasForeignKey(a => a.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuración de propiedades
            entity.Property(a => a.ProjectId)
                .IsRequired();

            entity.Property(a => a.ApplicantId)
                .IsRequired();

            entity.Property(a => a.ApplicantName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.ApplicantEmail)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(a => a.ApplicantPortfolio)
                .HasMaxLength(500);

            entity.Property(a => a.ApplicantPhone)
                .HasMaxLength(20);

            entity.Property(a => a.RoleId)
                .IsRequired();

            entity.Property(a => a.Message)
                .IsRequired()
                .HasMaxLength(1000);

            entity.Property(a => a.AcceptedTerms)
                .IsRequired();

            entity.Property(a => a.CvFileName)
                .HasMaxLength(255);

            entity.Property(a => a.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.Property(a => a.ReviewNotes)
                .HasMaxLength(1000);

            entity.Property(a => a.ReviewerId)
                .HasDefaultValue(0);

            entity.Property(a => a.CreatedAt)
                .IsRequired();

            entity.Property(a => a.UpdatedAt)
                .IsRequired();

            entity.Property(a => a.ReviewedAt)
                .IsRequired(false);

            // Índices
            entity.HasIndex(a => a.ProjectId);
            entity.HasIndex(a => a.ApplicantId);
            entity.HasIndex(a => a.RoleId);
            entity.HasIndex(a => a.Status);
            entity.HasIndex(a => a.CreatedAt);
        });

        // Configuración de la entidad Favorite
        builder.Entity<Favorite>(entity =>
        {
            // Llave primaria
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Configuración de propiedades
            entity.Property(f => f.ProfileId)
                .IsRequired();

            entity.Property(f => f.ProjectId)
                .IsRequired();

            entity.Property(f => f.CreatedAt)
                .IsRequired();

            // Índice único para evitar duplicados
            entity.HasIndex(f => new { f.ProfileId, f.ProjectId })
                .IsUnique();

            // Índices individuales
            entity.HasIndex(f => f.ProfileId);
            entity.HasIndex(f => f.ProjectId);
        });

        // Configuración de la entidad Rol
        builder.Entity<Rol>(entity =>
        {
            // Llave primaria
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Relación con Project (opcional)
            entity.HasOne<Project>()
                .WithMany(p => p.Roles)
                .HasForeignKey(r => r.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de propiedades
            entity.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(r => r.ProjectId)
                .IsRequired(false);

            entity.Property(r => r.CreatedAt)
                .IsRequired();

            entity.Property(r => r.UpdatedAt)
                .IsRequired();
        });

        // Configuración de la entidad RoleCard
        builder.Entity<RoleCard>(entity =>
        {
            // Llave primaria
            entity.HasKey(rc => rc.Id);
            entity.Property(rc => rc.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Relación con Rol
            entity.HasOne<Rol>()
                .WithMany(r => r.Cards)
                .HasForeignKey(rc => rc.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de propiedades
            entity.Property(rc => rc.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(rc => rc.Items)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");

            entity.Property(rc => rc.RoleId)
                .IsRequired(false);

            entity.Property(rc => rc.CreatedAt)
                .IsRequired();

            entity.Property(rc => rc.UpdatedAt)
                .IsRequired();
        });

        // Configuración de la entidad AcademicLevel (Value Object como entidad)
        builder.Entity<AcademicLevel>(entity =>
        {
            entity.HasKey(al => al.Id);
            entity.Property(al => al.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(al => al.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(al => al.Description)
                .HasMaxLength(500);

            entity.Property(al => al.Level)
                .IsRequired();

            entity.Property(al => al.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(al => al.CreatedAt)
                .IsRequired();

            entity.Property(al => al.UpdatedAt)
                .IsRequired();

            // Índices
            entity.HasIndex(al => al.Name).IsUnique();
            entity.HasIndex(al => al.Level);
            entity.HasIndex(al => al.IsActive);
        });

        // Configuración de la entidad DurationType (Value Object como entidad)
        builder.Entity<DurationType>(entity =>
        {
            entity.HasKey(dt => dt.Id);
            entity.Property(dt => dt.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            entity.Property(dt => dt.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(dt => dt.Value)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(dt => dt.Multiplier)
                .IsRequired();

            entity.Property(dt => dt.Active)
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(dt => dt.CreatedAt)
                .IsRequired();

            entity.Property(dt => dt.UpdatedAt)
                .IsRequired();

            // Índices
            entity.HasIndex(dt => dt.Name).IsUnique();
            entity.HasIndex(dt => dt.Active);
        });
    }
}