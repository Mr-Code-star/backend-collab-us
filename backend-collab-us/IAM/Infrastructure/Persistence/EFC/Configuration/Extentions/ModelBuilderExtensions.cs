using backend_collab_us.IAM.domain.model.agregates;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.IAM.Infrastructure.Persistence.EFC.Configuration.Extentions;

public static class ModelBuilderExtensions
{
    public static void ApplyIamConfiguration(this ModelBuilder builder)
    {
        // Configuración de la entidad User
        builder.Entity<User>(entity =>
        {
            // Llave primaria
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            // Configuración de propiedades
            entity.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(254); // Longitud máxima estándar para emails

            entity.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255); // Longitud adecuada para contraseñas hasheadas

            entity.Property(u => u.Status)
                .IsRequired()
                .HasMaxLength(20); // Longitud típica para estados

            entity.Property(u => u.CreatedAt)
                .IsRequired();

            entity.Property(u => u.UpdatedAt)
                .IsRequired();

            // Índices para mejorar performance
            entity.HasIndex(u => u.Email)
                .IsUnique(); // El email debe ser único

            entity.HasIndex(u => u.Status);
        });
    }
}