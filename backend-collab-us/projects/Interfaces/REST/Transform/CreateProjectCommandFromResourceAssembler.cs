using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.model.valueObjects;
using backend_collab_us.projects.Interfaces.REST.Resources;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class CreateProjectCommandFromResourceAssembler
{
    public static CreateProjectCommand ToCommandFromResource(CreateProjectResource resource, AppDbContext context)
    {
        // CARGAR AcademicLevel DESDE LA BASE DE DATOS
        var academicLevel = context.AcademicLevels
                                .FirstOrDefault(al => al.Name == resource.AcademicLevelName) 
                            ?? new AcademicLevel { Name = resource.AcademicLevelName ?? "Estudiante Universitario" };

        // CARGAR DurationType DESDE LA BASE DE DATOS
        var durationType = context.DurationTypes
                               .FirstOrDefault(dt => dt.Value == resource.DurationType) 
                           ?? new DurationType { Value = resource.DurationType ?? "meses" };

        // Convertir roles
        var roleCommands = resource.Roles.Select(roleResource => 
            new CreateRoleCommand(
                roleResource.Name,
                roleResource.Cards.Select(cardResource => 
                    new CreateRoleCardCommand(cardResource.Title, cardResource.Items)
                ).ToList()
            )
        ).ToList();

        return new CreateProjectCommand(
            resource.UserId,
            resource.Title,
            resource.Description,
            resource.Summary,
            academicLevel,
            resource.Benefits,
            resource.Skills,
            resource.DurationQuantity,
            durationType,
            resource.Areas,
            roleCommands,
            resource.Tags,
            resource.Status,
            resource.Progress
        );
    }
}