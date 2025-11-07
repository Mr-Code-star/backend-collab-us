using System.Net.Mime;
using backend_collab_us.task_management.domain.model.agregates;
using backend_collab_us.task_management.domain.model.commands;
using backend_collab_us.task_management.domain.model.queries;
using backend_collab_us.task_management.domain.Services;
using backend_collab_us.task_management.Interfaces.REST.Resources;
using backend_collab_us.task_management.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.task_management.Interfaces;

[ApiController]
[Route("api/v1/task-submissions")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Task Submissions Management")]
public class TaskSubmissionsController(
    ITaskSubmissionCommandService taskSubmissionCommandService,
    ITaskSubmissionQueryService taskSubmissionQueryService
) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Task Submissions",
        Description = "Returns all task submissions with filtering options",
        OperationId = "GetAllTaskSubmissions")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of task submissions", typeof(IEnumerable<TaskSubmissionResource>))]
    public async Task<IActionResult> GetAllTaskSubmissions(
        [FromQuery] int? taskId = null,
        [FromQuery] int? collaboratorId = null,
        [FromQuery] int? projectId = null)
    {
        try
        {
            IEnumerable<TaskSubmission> submissions;

            if (taskId.HasValue)
            {
                submissions = await taskSubmissionQueryService.Handle(new GetSubmissionsByTaskIdQuery(taskId.Value));
            }
            else if (collaboratorId.HasValue)
            {
                submissions = await taskSubmissionQueryService.Handle(new GetSubmissionsByCollaboratorIdQuery(collaboratorId.Value));
            }
            else if (projectId.HasValue)
            {
                submissions = await taskSubmissionQueryService.Handle(new GetSubmissionsByProjectIdQuery(projectId.Value));
            }
            else
            {
                // Return empty list or implement pagination for all submissions
                submissions = new List<TaskSubmission>();
            }

            var resources = submissions.Select(TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting task submissions: {ex.Message}");
            return BadRequest($"Error getting task submissions: {ex.Message}");
        }
    }

    [HttpGet("{submissionId}")]
    [SwaggerOperation(
        Summary = "Get Task Submission by Id",
        Description = "Returns a specific task submission by its ID",
        OperationId = "GetTaskSubmissionById")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task submission found", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> GetTaskSubmissionById([FromRoute] int submissionId)
    {
        try
        {
            var submission = await taskSubmissionQueryService.Handle(new GetTaskSubmissionByIdQuery(submissionId));
            
            if (submission is null)
                return NotFound("Task submission not found");

            var resource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return Ok(resource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting task submission: {ex.Message}");
            return BadRequest($"Error getting task submission: {ex.Message}");
        }
    }

    [HttpGet("pending-review")]
    [SwaggerOperation(
        Summary = "Get Pending Review Submissions",
        Description = "Returns all submissions pending review for a project",
        OperationId = "GetPendingReviewSubmissions")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of pending review submissions", typeof(IEnumerable<TaskSubmissionResource>))]
    public async Task<IActionResult> GetPendingReviewSubmissions([FromQuery] int projectId)
    {
        try
        {
            var submissions = await taskSubmissionQueryService.Handle(new GetPendingReviewSubmissionsQuery(projectId));
            var resources = submissions.Select(TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting pending review submissions: {ex.Message}");
            return BadRequest($"Error getting pending review submissions: {ex.Message}");
        }
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Task Submission",
        Description = "Creates a new task submission",
        OperationId = "CreateTaskSubmission")]
    [SwaggerResponse(StatusCodes.Status201Created, "Task submission created successfully", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task submission could not be created")]
    public async Task<IActionResult> CreateTaskSubmission([FromBody] CreateTaskSubmissionResource resource)
    {
        try
        {
            var command = CreateTaskSubmissionCommandFromResourceAssembler.ToCommandFromResource(resource);
            var submission = await taskSubmissionCommandService.Handle(command);
            
            if (submission is null)
                return BadRequest("Task submission could not be created");

            var submissionResource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return CreatedAtAction(nameof(GetTaskSubmissionById), new { submissionId = submissionResource.Id }, submissionResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating task submission: {ex.Message}");
            return BadRequest($"Task submission could not be created: {ex.Message}");
        }
    }

    [HttpPut("{submissionId}")]
    [SwaggerOperation(
        Summary = "Update Task Submission",
        Description = "Updates an existing task submission",
        OperationId = "UpdateTaskSubmission")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task submission updated successfully", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task submission could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> UpdateTaskSubmission(
        [FromRoute] int submissionId,
        [FromBody] UpdateTaskSubmissionResource resource)
    {
        try
        {
            var command = CreateTaskSubmissionCommandFromResourceAssembler.ToCommandFromResource(submissionId, resource);
            var submission = await taskSubmissionCommandService.Handle(command);
            
            if (submission is null)
                return NotFound("Task submission not found");

            var submissionResource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return Ok(submissionResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating task submission: {ex.Message}");
            return BadRequest($"Task submission could not be updated: {ex.Message}");
        }
    }

    [HttpPatch("{submissionId}/review")]
    [SwaggerOperation(
        Summary = "Review Task Submission",
        Description = "Marks a task submission as reviewed",
        OperationId = "ReviewTaskSubmission")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task submission reviewed successfully", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task submission could not be reviewed")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> ReviewTaskSubmission(
        [FromRoute] int submissionId,
        [FromBody] ReviewTaskSubmissionResource resource)
    {
        try
        {
            var command = CreateTaskSubmissionCommandFromResourceAssembler.ToCommandFromResource(submissionId, resource);
            var submission = await taskSubmissionCommandService.Handle(command);
            
            if (submission is null)
                return NotFound("Task submission not found");

            var submissionResource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return Ok(submissionResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reviewing task submission: {ex.Message}");
            return BadRequest($"Task submission could not be reviewed: {ex.Message}");
        }
    }

    [HttpPatch("{submissionId}/request-revision")]
    [SwaggerOperation(
        Summary = "Request Submission Revision",
        Description = "Requests revision for a task submission",
        OperationId = "RequestSubmissionRevision")]
    [SwaggerResponse(StatusCodes.Status200OK, "Revision requested successfully", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Revision could not be requested")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> RequestSubmissionRevision(
        [FromRoute] int submissionId,
        [FromBody] RequestSubmissionRevisionResource resource)
    {
        try
        {
            var command = CreateTaskSubmissionCommandFromResourceAssembler.ToCommandFromResource(submissionId, resource);
            var submission = await taskSubmissionCommandService.Handle(command);
            
            if (submission is null)
                return NotFound("Task submission not found");

            var submissionResource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return Ok(submissionResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error requesting submission revision: {ex.Message}");
            return BadRequest($"Revision could not be requested: {ex.Message}");
        }
    }

    [HttpPatch("{submissionId}/resubmit")]
    [SwaggerOperation(
        Summary = "Resubmit Task Submission",
        Description = "Resubmits a task submission after revision",
        OperationId = "ResubmitTaskSubmission")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task submission resubmitted successfully", typeof(TaskSubmissionResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task submission could not be resubmitted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> ResubmitTaskSubmission(
        [FromRoute] int submissionId,
        [FromBody] ResubmitTaskSubmissionResource resource)
    {
        try
        {
            var command = CreateTaskSubmissionCommandFromResourceAssembler.ToCommandFromResource(submissionId, resource);
            var submission = await taskSubmissionCommandService.Handle(command);
            
            if (submission is null)
                return NotFound("Task submission not found");

            var submissionResource = TaskSubmissionResourceFromEntityAssembler.ToResourceFromEntity(submission);
            return Ok(submissionResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resubmitting task submission: {ex.Message}");
            return BadRequest($"Task submission could not be resubmitted: {ex.Message}");
        }
    }

    [HttpDelete("{submissionId}")]
    [SwaggerOperation(
        Summary = "Delete Task Submission",
        Description = "Deletes a task submission",
        OperationId = "DeleteTaskSubmission")]
    [SwaggerResponse(StatusCodes.Status200OK, "Task submission deleted successfully")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Task submission could not be deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Task submission not found")]
    public async Task<IActionResult> DeleteTaskSubmission(
        [FromRoute] int submissionId,
        [FromQuery] int deletedBy)
    {
        try
        {
            var command = new DeleteTaskSubmissionCommand(submissionId, deletedBy);
            var result = await taskSubmissionCommandService.Handle(command);
            
            if (!result)
                return NotFound("Task submission not found");

            return Ok(new { message = "Task submission deleted successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting task submission: {ex.Message}");
            return BadRequest($"Task submission could not be deleted: {ex.Message}");
        }
    }
}