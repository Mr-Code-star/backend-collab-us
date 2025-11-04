using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class ProjectResourceFromEntityAssembler
{
    // ✅ Mantén el método original para compatibilidad
    public static ProjectResource ToResourceFromEntity(Project project)
    {
        return ToResourceFromEntity(project, "Usuario"); // Valor por defecto
    }

    // ✅ Método privado que acepta el authorName
private static ProjectResource ToResourceFromEntity(Project project, string authorName)
{
    var academicLevelName = project.AcademicLevelName?.Name ?? 
                            project.AcademicLevelName?.ToString() ?? 
                            "No especificado";

    var durationTypeName = project.DurationType?.Name ?? 
                           project.DurationType?.Value ?? 
                           "meses";

    var roleResources = project.Roles?.Select(role =>
        new RoleResource(
            role.Id,
            role.Name,
            role.Cards?.Select(card =>
                new RoleCardResource(
                    card.Id,
                    card.Title,
                    card.Items ?? new List<string>(),
                    card.CreatedAt,
                    card.UpdatedAt
                )
            ).ToList() ?? new List<RoleCardResource>(),
            role.CreatedAt,
            role.UpdatedAt
        )
    ).ToList() ?? new List<RoleResource>();

    // ✅ CORREGIR: Incluir colaboradores en el Resource
    var collaboratorResources = project.Collaborators?
        .Where(collab => collab.Status == "accepted") // ✅ Solo colaboradores aceptados
        .Select(collab =>
            new ApplicationResource(
                collab.Id,
                collab.ProjectId,
                collab.ApplicantId,
                collab.ApplicantName,
                collab.ApplicantEmail,
                collab.ApplicantPortfolio ?? "",
                collab.ApplicantPhone ?? "",
                collab.RoleId,
                collab.Message,
                collab.AcceptedTerms,
                collab.CvFileName ?? "",
                collab.Status,
                collab.ReviewNotes ?? "",
                collab.ReviewerId,
                collab.CreatedAt,
                collab.UpdatedAt,
                collab.ReviewedAt
            )
        ).ToList() ?? new List<ApplicationResource>();

    return new ProjectResource(
        project.Id,
        project.UserId,
        project.Title,
        project.Description,
        project.Summary,
        academicLevelName,
        project.Benefits,
        project.Skills ?? new List<string>(),
        project.DurationQuantity,
        durationTypeName,
        project.Status,
        project.Progress,
        roleResources,
        project.Areas?.ToList() ?? new List<string>(),
        project.Tags?.ToList() ?? new List<string>(),
        project.CreatedAt,
        project.UpdatedAt,
        authorName,
        collaboratorResources 
    );
}
    // ✅ NUEVO: Método async que busca el perfil
    public static async Task<ProjectResource> ToResourceFromEntityAsync(
        Project project, 
        IProfileRepository profileRepository)
    {
        string authorName = "Usuario"; // Valor por defecto
        
        try
        {
            // ✅ Buscar el perfil del usuario que creó el proyecto
            var profile = await profileRepository.FindByUserIdAsync(project.UserId);
            authorName = profile?.Username ?? "Usuario";
        }
        catch (Exception ex)
        {
            // ✅ Manejar errores silenciosamente para no afectar la respuesta principal
            Console.WriteLine($"❌ Error buscando perfil para userId {project.UserId}: {ex.Message}");
        }

        // ✅ Usar el método privado con el authorName encontrado
        return ToResourceFromEntity(project, authorName);
    }
}