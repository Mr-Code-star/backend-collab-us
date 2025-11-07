using backend_collab_us.task_management.Interfaces.REST.Resources;
using Task = backend_collab_us.task_management.domain.model.agregates.Task;
namespace backend_collab_us.task_management.Interfaces.REST.Transform;

public static class TaskResourceFromEntityAssembler
{
    public static TaskResource ToResourceFromEntity(Task task)
    {
        var checklistResources = task.Checklist.Select(item => 
            new ChecklistItemResource(
                item.Id,
                item.Text,
                item.Completed,
                item.CreatedAt,
                item.UpdatedAt
            )
        ).ToList();

        var toolResources = task.Tools.Select(tool => 
            new TaskToolResource(
                tool.Id,
                tool.Name,
                tool.Checked,
                tool.CreatedAt,
                tool.UpdatedAt
            )
        ).ToList();

        var attachmentResources = task.Attachments.Select(attachment => 
            new TaskAttachmentResource(
                attachment.Id,
                attachment.Name,
                attachment.Type,
                attachment.Url,
                attachment.Icon,
                attachment.UploadedAt
            )
        ).ToList();

        return new TaskResource(
            task.Id,
            task.Title,
            task.Description,
            task.DueDate,
            task.Status,
            task.Priority,
            task.ProjectId,
            task.AssignedTo,
            task.AssignedToName,
            task.Role,
            task.Comment,
            task.Progress,
            task.EstimatedHours,
            task.ActualHours,
            task.CreatedBy,
            task.CreatedAt,
            task.UpdatedAt,
            task.CompletedAt,
            checklistResources,
            toolResources,
            attachmentResources
        );
    }
}