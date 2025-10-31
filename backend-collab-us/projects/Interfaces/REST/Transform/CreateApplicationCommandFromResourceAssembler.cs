using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class CreateApplicationCommandFromResourceAssembler
{
    public static CreateApplicationCommand ToCommandFromResource(CreateApplicationResource resource)
    {
        return new CreateApplicationCommand(
            resource.ProjectId,
            resource.ApplicantId,
            resource.ApplicantName,
            resource.ApplicantEmail,
            resource.ApplicantPortfolio,
            resource.ApplicantPhone,
            resource.RoleId,
            resource.Message,
            resource.AcceptedTerms,
            resource.CvFileName,
            resource.Status
        );
    }
}