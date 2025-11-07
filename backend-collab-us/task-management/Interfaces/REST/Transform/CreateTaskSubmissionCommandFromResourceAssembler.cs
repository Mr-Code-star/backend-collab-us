using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.Interfaces.REST.Resources;

namespace backend_collab_us.task_management.Interfaces.REST.Transform;

public static class CreateTaskSubmissionCommandFromResourceAssembler
{
    public static CreateTaskSubmissionCommand ToCommandFromResource(CreateTaskSubmissionResource resource)
    {
        var linkCommands = resource.Links?.Select(link => 
            new CreateSubmissionLinkCommand(link.Url, link.Description)
        ).ToList();

        var attachmentCommands = resource.Attachments?.Select(attachment => 
            new CreateSubmissionAttachmentCommand(
                attachment.Name,
                attachment.Type,
                attachment.Url,
                attachment.Size
            )
        ).ToList();

        return new CreateTaskSubmissionCommand(
            resource.TaskId,
            resource.CollaboratorId,
            resource.CollaboratorName,
            resource.Notes,
            linkCommands,
            attachmentCommands
        );
    }

    public static UpdateTaskSubmissionCommand ToCommandFromResource(int submissionId, UpdateTaskSubmissionResource resource)
    {
        var linkCommands = resource.Links?.Select(link => 
            new CreateSubmissionLinkCommand(link.Url, link.Description)
        ).ToList();

        var attachmentCommands = resource.Attachments?.Select(attachment => 
            new CreateSubmissionAttachmentCommand(
                attachment.Name,
                attachment.Type,
                attachment.Url,
                attachment.Size
            )
        ).ToList();

        return new UpdateTaskSubmissionCommand(
            submissionId,
            resource.Notes,
            linkCommands,
            attachmentCommands
        );
    }

    public static ReviewTaskSubmissionCommand ToCommandFromResource(int submissionId, ReviewTaskSubmissionResource resource)
    {
        return new ReviewTaskSubmissionCommand(
            submissionId,
            resource.ReviewerId,
            resource.ReviewNotes
        );
    }

    public static RequestSubmissionRevisionCommand ToCommandFromResource(int submissionId, RequestSubmissionRevisionResource resource)
    {
        return new RequestSubmissionRevisionCommand(
            submissionId,
            resource.RevisionNotes
        );
    }

    public static ResubmitTaskSubmissionCommand ToCommandFromResource(int submissionId, ResubmitTaskSubmissionResource resource)
    {
        var linkCommands = resource.NewLinks?.Select(link => 
            new CreateSubmissionLinkCommand(link.Url, link.Description)
        ).ToList();

        var attachmentCommands = resource.NewAttachments?.Select(attachment => 
            new CreateSubmissionAttachmentCommand(
                attachment.Name,
                attachment.Type,
                attachment.Url,
                attachment.Size
            )
        ).ToList();

        return new ResubmitTaskSubmissionCommand(
            submissionId,
            resource.Notes,
            linkCommands,
            attachmentCommands
        );
    }
}