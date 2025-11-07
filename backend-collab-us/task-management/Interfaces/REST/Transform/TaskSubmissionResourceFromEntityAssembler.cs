using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.Interfaces.REST.Resources;

namespace backend_collab_us.task_management.Interfaces.REST.Transform;

public static class TaskSubmissionResourceFromEntityAssembler
{
    public static TaskSubmissionResource ToResourceFromEntity(TaskSubmission submission)
    {
        var linkResources = submission.Links.Select(link => 
            new SubmissionLinkResource(
                link.Id,
                link.Url,
                link.Description,
                link.CreatedAt
            )
        ).ToList();

        var attachmentResources = submission.Attachments.Select(attachment => 
            new SubmissionAttachmentResource(
                attachment.Id,
                attachment.Name,
                attachment.Type,
                attachment.Url,
                attachment.Size,
                attachment.UploadedAt
            )
        ).ToList();

        return new TaskSubmissionResource(
            submission.Id,
            submission.TaskId,
            submission.CollaboratorId,
            submission.CollaboratorName,
            submission.SubmittedAt,
            linkResources,
            attachmentResources,
            submission.Notes,
            submission.Status,
            submission.ReviewedAt,
            submission.ReviewerId,
            submission.ReviewNotes,
            submission.CreatedAt,
            submission.UpdatedAt
        );
    }
}