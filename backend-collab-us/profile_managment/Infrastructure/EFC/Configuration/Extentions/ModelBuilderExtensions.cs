using backend_collab_us.profile_managment.domain.model.agregates;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using backend_collab_us.profile_managment.domain.model.valueObjects;
using backend_collab_us.IAM.domain.model.agregates; // Agregar referencia

namespace backend_collab_us.profile_managment.Infrastructure.Persistence.EFC.Configuration.Extentions;

public static class ModelBuilderExtensions
{
    public static void ApplyProfileManagementConfiguration(this ModelBuilder builder)
    {
        // Configuración de la entidad Profile
        builder.Entity<Profile>(entity =>
        {
            // Llave primaria - CAMBIADO a int
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id)
                .IsRequired()
                .ValueGeneratedOnAdd(); // Ahora sí puede ser autoincremental

            // AGREGAR: Relación con User
            entity.HasOne(p => p.User)
                .WithMany() // Si un User puede tener múltiples Profiles, cambia a .WithMany(p => p.Profiles)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade); // O Restrict según tu necesidad

            // Configuración de propiedades
            entity.Property(p => p.UserId)
                .IsRequired();

            entity.Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(50);

            // ... el resto de la configuración se mantiene igual ...
            entity.Property(p => p.Avatar)
                .HasMaxLength(500);

            entity.Property(p => p.Role)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(p => p.Bio)
                .HasMaxLength(1000);

            entity.Property(p => p.Abilities)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");

            entity.Property(p => p.Experiences)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<Experience>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");

            entity.Property(p => p.Cv)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<CV>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");
            
            entity.Property(p => p.PointsGivenBy)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null)
                )
                .HasColumnType("text");

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(p => p.Points)
                .IsRequired();

            entity.Property(p => p.CreatedAt)
                .IsRequired();

            entity.Property(p => p.UpdatedAt)
                .IsRequired();

            // Índices
            entity.HasIndex(p => p.UserId)
                .IsUnique();

            entity.HasIndex(p => p.Username)
                .IsUnique();

            entity.HasIndex(p => p.Status);

            entity.HasIndex(p => p.Role);
        });
    }
}