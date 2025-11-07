using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.Interfaces.REST.Resources;

namespace backend_collab_us.task_management.Interfaces.REST.Transform;

public static class CreateTaskCommandFromResourceAssembler
{
    public static CreateTaskCommand ToCommandFromResource(CreateTaskResource resource)
    {
        var checklistCommands = resource.Checklist?.Select(item => 
            new CreateChecklistItemCommand(item.Text, item.Completed)
        ).ToList() ?? new List<CreateChecklistItemCommand>();

        var toolCommands = resource.Tools?.Select(tool => 
            new CreateTaskToolCommand(tool.Name, tool.Checked)
        ).ToList() ?? new List<CreateTaskToolCommand>();

        var attachmentCommands = resource.Attachments?.Select(attachment => 
            new CreateTaskAttachmentCommand(attachment.Name, attachment.Type, attachment.Url, attachment.Icon)
        ).ToList() ?? new List<CreateTaskAttachmentCommand>();

        return new CreateTaskCommand(
            resource.Title,
            resource.Description,
            resource.DueDate,
            resource.Status,
            resource.Priority,
            resource.ProjectId,
            resource.AssignedTo,
            resource.AssignedToName,
            resource.Role,
            checklistCommands,
            toolCommands,
            resource.Comment,
            attachmentCommands,
            resource.EstimatedHours,
            resource.CreatedBy
        );
    }
}