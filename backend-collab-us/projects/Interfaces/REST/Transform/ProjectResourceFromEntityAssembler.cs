using backend_collab_us.projects.domain.model.agregates;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class ProjectResourceFromEntityAssembler
{
    public static ProjectResource ToResourceFromEntity(Project project)
    {
        var roleResources = project.Roles.Select(role =>
            new RoleResource(
                role.Id,
                role.Name,
                role.Cards.Select(card =>
                    new RoleCardResource(
                        card.Id,
                        card.Title,
                        card.Items,
                        card.CreatedAt,
                        card.UpdatedAt
                    )
                ).ToList(),
                role.CreatedAt,
                role.UpdatedAt
            )
        ).ToList();

        return new ProjectResource(
            project.Id,
            project.UserId,
            project.Title,
            project.Description,
            project.Summary,
            project.AcademicLevelName.ToString(),
            project.Benefits,
            project.Skills,
            project.DurationQuantity,
            project.DurationType.ToString(),
            project.Status,
            project.Progress,
            roleResources,
            project.Areas.ToList(),
            project.Tags.ToList(),
            project.CreatedAt,
            project.UpdatedAt
        );
    }
}