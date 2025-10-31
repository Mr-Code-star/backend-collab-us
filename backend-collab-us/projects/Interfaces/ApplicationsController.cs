using System.Net.Mime;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.services;
using backend_collab_us.projects.Interfaces.REST.Resources;
using backend_collab_us.projects.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.projects.Interfaces;

[ApiController]
[Route("api/v1/applications")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Applications Endpoints")]
public class ApplicationsController(
    IApplicationCommandService applicationCommandService,
    IApplicationQueryService applicationQueryService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Applications",
        Description = "Returns a list of all applications with optional filtering.",
        OperationId = "GetAllApplications")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of applications", typeof(IEnumerable<ApplicationResource>))]
    public async Task<IActionResult> GetAllApplications(
        [FromQuery] int? projectId)
    {
        IEnumerable<domain.model.agregates.Application> applications;
        
        if (projectId.HasValue)
            applications = await applicationQueryService.Handle(new GetApplicationsByProjectIdQuery(projectId.Value));
        else
            applications = await applicationQueryService.Handle(new GetApplicationsByProjectIdQuery(0)); // Adjust as needed

        var resources = applications.Select(ApplicationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Application by Id",
        Description = "Returns an application by its unique identifier.",
        OperationId = "GetApplicationById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Application Found", typeof(ApplicationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Application not found")]
    public async Task<IActionResult> GetApplicationById([FromRoute] int id)
    {
        var application = await applicationQueryService.Handle(new GetApplicationByIdQuery(id));
        if (application is null) return NotFound();
        var resource = ApplicationResourceFromEntityAssembler.ToResourceFromEntity(application);
        return Ok(resource);
    }

    [HttpGet("project/{projectId}")]
    [SwaggerOperation(
        Summary = "Get Applications by Project",
        Description = "Returns all applications for a specific project.",
        OperationId = "GetApplicationsByProject"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of applications for the project", typeof(IEnumerable<ApplicationResource>))]
    public async Task<IActionResult> GetApplicationsByProject([FromRoute] int projectId)
    {
        var applications = await applicationQueryService.Handle(new GetApplicationsByProjectIdQuery(projectId));
        var resources = applications.Select(ApplicationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Application",
        Description = "Creates a new application to a project.",
        OperationId = "CreateApplication")]
    [SwaggerResponse(StatusCodes.Status201Created, "Application created successfully", typeof(ApplicationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Application could not be created")]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationResource resource)
    {
        try
        {
            var command = CreateApplicationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var application = await applicationCommandService.Handle(command);
        
            if (application is null) 
                return BadRequest("Application could not be created. Possible reasons: Project doesn't exist, applicant doesn't exist, or duplicate application.");
        
            var applicationResource = ApplicationResourceFromEntityAssembler.ToResourceFromEntity(application);
            return CreatedAtAction(nameof(GetApplicationById), new { id = applicationResource.Id }, applicationResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateApplication endpoint: {ex.Message}");
            return BadRequest($"Application could not be created: {ex.Message}");
        }
    }

    [HttpPatch("{id}/status")]
    [SwaggerOperation(
        Summary = "Update Application Status",
        Description = "Updates the status of an application (accept/reject)",
        OperationId = "UpdateApplicationStatus"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Status updated successfully", typeof(ApplicationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Status could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Application not found")]
    public async Task<IActionResult> UpdateApplicationStatus(
        [FromRoute] int id, 
        [FromBody] UpdateApplicationStatusResource resource)
    {
        try
        {
            var command = UpdateApplicationStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var application = await applicationCommandService.Handle(command);
            
            if (application is null) 
                return NotFound("Application not found");
            
            var applicationResource = ApplicationResourceFromEntityAssembler.ToResourceFromEntity(application);
            return Ok(applicationResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateApplicationStatus endpoint: {ex.Message}");
            return BadRequest($"Status could not be updated: {ex.Message}");
        }
    }

    [HttpPost("{id}/accept")]
    [SwaggerOperation(
        Summary = "Accept Application",
        Description = "Accepts an application",
        OperationId = "AcceptApplication"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Application accepted", typeof(ApplicationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Application could not be accepted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Application not found")]
    public async Task<IActionResult> AcceptApplication(
        [FromRoute] int id,
        [FromBody] AcceptApplicationResource resource)
    {
        try
        {
            var command = new AcceptApplicationCommand(id, resource.ReviewerId, resource.ReviewNotes);
            var application = await applicationCommandService.Handle(command);
            
            if (application is null) 
                return NotFound("Application not found");
            
            var applicationResource = ApplicationResourceFromEntityAssembler.ToResourceFromEntity(application);
            return Ok(applicationResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AcceptApplication endpoint: {ex.Message}");
            return BadRequest($"Application could not be accepted: {ex.Message}");
        }
    }

    [HttpPost("{id}/reject")]
    [SwaggerOperation(
        Summary = "Reject Application",
        Description = "Rejects an application",
        OperationId = "RejectApplication"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Application rejected", typeof(ApplicationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Application could not be rejected")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Application not found")]
    public async Task<IActionResult> RejectApplication(
        [FromRoute] int id,
        [FromBody] RejectApplicationResource resource)
    {
        try
        {
            var command = new RejectApplicationCommand(id, resource.ReviewerId, resource.ReviewNotes);
            var application = await applicationCommandService.Handle(command);
            
            if (application is null) 
                return NotFound("Application not found");
            
            var applicationResource = ApplicationResourceFromEntityAssembler.ToResourceFromEntity(application);
            return Ok(applicationResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RejectApplication endpoint: {ex.Message}");
            return BadRequest($"Application could not be rejected: {ex.Message}");
        }
    }
    
}