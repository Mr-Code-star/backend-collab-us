using System.Net.Mime;
using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.model.queries;
using backend_collab_us.task_management.domain.Services;
using backend_collab_us.task_management.Interfaces.REST.Resources;
using backend_collab_us.task_management.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.task_management.Interfaces;

[ApiController]
[Route("api/v1/projects/{projectId}/tasks")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Tasks Endpoints")]
public class TasksController(
    ITaskCommandService taskCommandService,
    ITaskQueryService taskQueryService
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get Project Tasks",
        Description = "Returns all tasks for a specific project",
        OperationId = "GetProjectTasks")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of tasks", typeof(IEnumerable<TaskResource>))]
    public async Task<IActionResult> GetProjectTasks([FromRoute] int projectId)
    {
        var tasks = await taskQueryService.Handle(new GetProjectTasksQuery(projectId));
        var resources = tasks.Select(TaskResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search Tasks",
        Description = "Search tasks within a project by assigned name and/or status",
        OperationId = "SearchTasks")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of matching tasks", typeof(IEnumerable<TaskResource>))]
    public async Task<IActionResult> SearchTasks(
        [FromRoute] int projectId,
        [FromQuery] string? assignedToName = null,
        [FromQuery] string? status = null)
    {
        var tasks = await taskQueryService.Handle(new SearchTasksQuery(projectId, assignedToName, status));
        var resources = tasks.Select(TaskResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{taskId}")]
    [SwaggerOperation(
        Summary = "Get Task by Id",
        Description = "Returns a specific task by its ID",
        OperationId = "GetTaskById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task found", typeof(TaskResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task not found")]
    public async Task<IActionResult> GetTaskById([FromRoute] int projectId, [FromRoute] int taskId)
    {
        var tasks = await taskQueryService.Handle(new GetProjectTasksQuery(projectId));
        var task = tasks.FirstOrDefault(t => t?.Id == taskId);
        
        if (task is null) return NotFound();
        
        var resource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
        return Ok(resource);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Task",
        Description = "Creates a new task for the project",
        OperationId = "CreateTask")]
    [SwaggerResponse(StatusCodes.Status201Created, "Task created successfully", typeof(TaskResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task could not be created")]
    public async Task<IActionResult> CreateTask(
        [FromRoute] int projectId,
        [FromBody] CreateTaskResource resource)
    {
        try
        {
            // Ensure the projectId in route matches the resource
            var commandResource = resource with { ProjectId = projectId };
            var command = CreateTaskCommandFromResourceAssembler.ToCommandFromResource(commandResource);
            
            var task = await taskCommandService.Handle(command);
            
            if (task is null)
                return BadRequest("Task could not be created");

            var taskResource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
            return CreatedAtAction(nameof(GetTaskById), new { projectId, taskId = taskResource.Id }, taskResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateTask endpoint: {ex.Message}");
            return BadRequest($"Task could not be created: {ex.Message}");
        }
    }

    [HttpPost("bulk")]
    [SwaggerOperation(
        Summary = "Create Tasks for All Accepted Applicants",
        Description = "Creates tasks for all accepted applicants in the project",
        OperationId = "CreateTasksForAllAcceptedApplicants")]
    [SwaggerResponse(StatusCodes.Status201Created, "Tasks created successfully", typeof(IEnumerable<TaskResource>))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Tasks could not be created")]
    public async Task<IActionResult> CreateTasksForAllAcceptedApplicants(
        [FromRoute] int projectId,
        [FromBody] CreateTaskResource resource)
    {
        try
        {
            var commandResource = resource with { ProjectId = projectId };
            var baseCommand = CreateTaskCommandFromResourceAssembler.ToCommandFromResource(commandResource);
            
            var tasks = await taskCommandService.CreateTasksForAllAcceptedApplicants(baseCommand);
            
            var taskResources = tasks.Select(TaskResourceFromEntityAssembler.ToResourceFromEntity);
            return CreatedAtAction(nameof(GetProjectTasks), new { projectId }, taskResources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateTasksForAllAcceptedApplicants endpoint: {ex.Message}");
            return BadRequest($"Tasks could not be created: {ex.Message}");
        }
    }

    [HttpPut("{taskId}")]
    [SwaggerOperation(
        Summary = "Update Task",
        Description = "Updates an existing task",
        OperationId = "UpdateTask")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task updated successfully", typeof(TaskResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task not found")]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] UpdateTaskResource resource)
    {
        try
        {
            var command = new UpdateTaskCommand(
                taskId,
                projectId,
                resource.Title,
                resource.Description,
                resource.DueDate,
                resource.Status,
                resource.Priority,
                resource.AssignedTo,
                resource.AssignedToName,
                resource.Role,
                resource.Comment,
                resource.EstimatedHours,
                resource.ActualHours
            );

            var task = await taskCommandService.Handle(command);
            
            if (task is null)
                return NotFound("Task not found");

            var taskResource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
            return Ok(taskResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateTask endpoint: {ex.Message}");
            return BadRequest($"Task could not be updated: {ex.Message}");
        }
    }

    [HttpPatch("{taskId}/status")]
    [SwaggerOperation(
        Summary = "Update Task Status",
        Description = "Updates the status of a task",
        OperationId = "UpdateTaskStatus")]
    [SwaggerResponse(StatusCodes.Status200OK, "Status updated successfully", typeof(TaskResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Status could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task not found")]
    public async Task<IActionResult> UpdateTaskStatus(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] string status)
    {
        try
        {
            var command = new UpdateTaskStatusCommand(taskId, projectId, status);
            var task = await taskCommandService.Handle(command);
            
            if (task is null)
                return NotFound("Task not found");

            var taskResource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
            return Ok(taskResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateTaskStatus endpoint: {ex.Message}");
            return BadRequest($"Status could not be updated: {ex.Message}");
        }
    }

    [HttpPatch("{taskId}/due-date")]
    [SwaggerOperation(
        Summary = "Update Task Due Date",
        Description = "Updates the due date of a task",
        OperationId = "UpdateTaskDueDate")]
    [SwaggerResponse(StatusCodes.Status200OK, "Due date updated successfully", typeof(TaskResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Due date could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task not found")]
    public async Task<IActionResult> UpdateTaskDueDate(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] DateTime newDueDate,
        [FromQuery] int updatedBy)
    {
        try
        {
            var command = new UpdateTaskDueDateCommand(taskId, projectId, newDueDate, updatedBy);
            var task = await taskCommandService.Handle(command);
            
            if (task is null)
                return NotFound("Task not found");

            var taskResource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
            return Ok(taskResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateTaskDueDate endpoint: {ex.Message}");
            return BadRequest($"Due date could not be updated: {ex.Message}");
        }
    }

    [HttpDelete("{taskId}")]
    [SwaggerOperation(
        Summary = "Delete Task",
        Description = "Deletes a task from the project",
        OperationId = "DeleteTask")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task deleted successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task could not be deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task not found")]
    public async Task<IActionResult> DeleteTask(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromQuery] int deletedBy)
    {
        try
        {
            var command = new DeleteTaskCommand(taskId, projectId, deletedBy);
            var result = await taskCommandService.Handle(command);
            
            if (!result)
                return NotFound("Task not found");

            return Ok(new { message = "Task deleted successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in DeleteTask endpoint: {ex.Message}");
            return BadRequest($"Task could not be deleted: {ex.Message}");
        }
    }

    // Checklist endpoints
    [HttpPost("{taskId}/checklist")]
    [SwaggerOperation(
        Summary = "Add Checklist Item",
        Description = "Adds a new item to the task checklist",
        OperationId = "AddChecklistItem")]
    [SwaggerResponse(StatusCodes.Status200OK, "Checklist item added", typeof(TaskResource))]
    public async Task<IActionResult> AddChecklistItem(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromBody] string text)
    {
        var command = new AddChecklistItemCommand(taskId, projectId, text);
        var task = await taskCommandService.Handle(command);
        
        if (task is null) return NotFound();
        
        var resource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
        return Ok(resource);
    }

    [HttpDelete("{taskId}/checklist/{itemId}")]
    [SwaggerOperation(
        Summary = "Remove Checklist Item",
        Description = "Removes an item from the task checklist",
        OperationId = "RemoveChecklistItem")]
    [SwaggerResponse(StatusCodes.Status200OK, "Checklist item removed", typeof(TaskResource))]
    public async Task<IActionResult> RemoveChecklistItem(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromRoute] int itemId)
    {
        var command = new RemoveChecklistItemCommand(taskId, projectId, itemId);
        var task = await taskCommandService.Handle(command);
        
        if (task is null) return NotFound();
        
        var resource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
        return Ok(resource);
    }

    [HttpPatch("{taskId}/checklist/{itemId}/toggle")]
    [SwaggerOperation(
        Summary = "Toggle Checklist Item",
        Description = "Toggles the completion status of a checklist item",
        OperationId = "ToggleChecklistItem")]
    [SwaggerResponse(StatusCodes.Status200OK, "Checklist item toggled", typeof(TaskResource))]
    public async Task<IActionResult> ToggleChecklistItem(
        [FromRoute] int projectId,
        [FromRoute] int taskId,
        [FromRoute] int itemId)
    {
        var command = new ToggleChecklistItemCommand(taskId, projectId, itemId);
        var task = await taskCommandService.Handle(command);
        
        if (task is null) return NotFound();
        
        var resource = TaskResourceFromEntityAssembler.ToResourceFromEntity(task);
        return Ok(resource);
    }
}