using System.Net.Mime;
using backend_collab_us.profile_managment.domain.repositories;
using backend_collab_us.projects.domain.model.commands;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.services;
using backend_collab_us.projects.Interfaces.REST.Resources;
using backend_collab_us.projects.Interfaces.REST.Transform;
using backend_collab_us.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.projects.Interfaces;

[ApiController]
[Route("api/v1/projects")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Projects Endpoints")]
public class ProjectsController(
    IProjectCommandService projectCommandService,
    IProjectQueryService projectQueryService, 
    IProfileRepository profileRepository
    ) : ControllerBase
    
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Projects",
        Description = "Returns a list of all projects.",
        OperationId = "GetAllProjects")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of projects", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> GetAllProjects()
    {
        var projects = await projectQueryService.Handle(new GetAllProjectsQuery());
    
        // Necesitas el IProfileRepository aquí también
        var resources = new List<ProjectResource>();
        foreach (var project in projects)
        {
            var resource = await ProjectResourceFromEntityAssembler.ToResourceFromEntityAsync(project, profileRepository);
            resources.Add(resource);
        }
    
        return Ok(resources);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Project by Id",
        Description = "Returns a project by its unique identifier.",
        OperationId = "GetProjectById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Project Found", typeof(ProjectResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Project not found")]
    public async Task<IActionResult> GetProjectById([FromRoute] int id)
    {
        var project = await projectQueryService.Handle(new GetProjectByIdQuery(id));
        if (project is null) return NotFound();
    
        // ✅ DEBUG: Log para verificar colaboradores
        Console.WriteLine($"🔍 Project {id} collaborators count: {project.Collaborators?.Count ?? 0}");
        if (project.Collaborators != null)
        {
            foreach (var collaborator in project.Collaborators)
            {
                Console.WriteLine($"👥 Collaborator: {collaborator.ApplicantName}, Status: {collaborator.Status}");
            }
        }
    
        var resource = await ProjectResourceFromEntityAssembler.ToResourceFromEntityAsync(
            project, 
            profileRepository);
        return Ok(resource);
    }
    
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search Projects",
        Description = "Returns a list of projects that match the search criteria.",
        OperationId = "SearchProjects"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of projects that match the search criteria", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> SearchProjects(
        [FromQuery] string? query,
        [FromQuery] string? area,
        [FromQuery] string? roleName,
        [FromQuery] string? status = "published")
    {
        var searchQuery = new SearchProjectsQuery(query, area, roleName, status);
        var projects = await projectQueryService.Handle(searchQuery);
        var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("area/{area}")]
    [SwaggerOperation(
        Summary = "Get Projects by Area",
        Description = "Returns projects by specific area.",
        OperationId = "GetProjectsByArea"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of projects in the area", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> GetProjectsByArea([FromRoute] string area)
    {
        var projects = await projectQueryService.Handle(new GetProjectsByAreaQuery(area));
        var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("role/{roleName}")]
    [SwaggerOperation(
        Summary = "Get Projects by Role",
        Description = "Returns projects that require a specific role.",
        OperationId = "GetProjectsByRole"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of projects for the role", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> GetProjectsByRole([FromRoute] string roleName)
    {
        var projects = await projectQueryService.Handle(new GetProjectsByRoleQuery(roleName));
        var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Project",
        Description = "Creates a new project and returns the created project resource.",
        OperationId = "CreateProject")]
    [SwaggerResponse(StatusCodes.Status201Created, "Project created successfully", typeof(ProjectResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Project could not be created")]
    public async Task<IActionResult> CreateProject(
        [FromBody] CreateProjectResource resource,
        [FromServices] AppDbContext context) // Inyectar el contexto
    {
        try
        {
            var command = CreateProjectCommandFromResourceAssembler.ToCommandFromResource(resource, context);
            var project = await projectCommandService.Handle(command);
    
            if (project is null) 
                return BadRequest("Project could not be created. Possible reasons: UserId doesn't exist or title is already taken.");
    
            var projectResource = ProjectResourceFromEntityAssembler.ToResourceFromEntity(project);
            return CreatedAtAction(nameof(GetProjectById), new { id = projectResource.Id }, projectResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateProject endpoint: {ex.Message}");
            return BadRequest($"Project could not be created: {ex.Message}");
        }
    }

    [HttpPatch("{id}/status")]
    [SwaggerOperation(
        Summary = "Update Project Status",
        Description = "Updates the status of a project",
        OperationId = "UpdateProjectStatus"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Status updated successfully", typeof(ProjectResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Status could not be updated")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Project not found")]
    public async Task<IActionResult> UpdateProjectStatus(
        [FromRoute] int id, 
        [FromBody] string status)
    {
        try
        {
            var command = new UpdateProjectStatusCommand(id, status);
            var project = await projectCommandService.Handle(command);
            
            if (project is null) 
                return NotFound("Project not found");
            
            var projectResource = ProjectResourceFromEntityAssembler.ToResourceFromEntity(project);
            return Ok(projectResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UpdateProjectStatus endpoint: {ex.Message}");
            return BadRequest($"Status could not be updated: {ex.Message}");
        }
    }
    
}