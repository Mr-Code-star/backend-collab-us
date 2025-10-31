
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class ApplicationResourceFromEntityAssembler
{
    public static ApplicationResource ToResourceFromEntity(domain.model.agregates.Application application)
    {
        return new ApplicationResource(
            application.Id,
            application.ProjectId,
            application.ApplicantId,
            application.ApplicantName,
            application.ApplicantEmail,
            application.ApplicantPortfolio,
            application.ApplicantPhone,
            application.RoleId,
            application.Message,
            application.AcceptedTerms,
            application.CvFileName,
            application.Status,
            application.ReviewNotes,
            application.ReviewerId,
            application.CreatedAt,
            application.UpdatedAt,
            application.ReviewedAt
        );
    }
}