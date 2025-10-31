using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.Interfaces.REST.Resources;

namespace backend_collab_us.projects.Interfaces.REST.Transform;

public static class UpdateApplicationStatusCommandFromResourceAssembler
{
    public static UpdateApplicationStatusCommand ToCommandFromResource(
        int applicationId, 
        UpdateApplicationStatusResource resource)
    {
        return new UpdateApplicationStatusCommand(
            applicationId,
            resource.Status,
            resource.ReviewNotes,
            resource.ReviewerId
        );
    }
}