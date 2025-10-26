using System.Net.Mime;
using backend_collab_us.profile_managment.domain.model.commands;
using backend_collab_us.profile_managment.domain.model.queries;
using backend_collab_us.profile_managment.Interfaces.REST.Resources;
using backend_collab_us.profile_managment.domain.services;
using backend_collab_us.profile_managment.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.profile_managment.Interfaces.REST;
[ApiController]
[Route("api/v1/profile")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available profile Endpoints")]
public class ProfileController(
    IProfileCommandService profileCommandService,
    IProfileQueryService profileQueryService) : ControllerBase
{
    [HttpGet("{id}")]
    [SwaggerOperation(
        Summary = "Get Profile by Id",
        Description = "Returns a profile by its unique identifier.",
        OperationId = "GetProfileById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Profile Found", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found")]
    public async Task<IActionResult> GetProfileById([FromRoute] int id) // CAMBIAR string por int
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(id));
        if (profile is null) return NotFound();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(resource);
    }
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Profiles",
        Description = "Returns a list of all profiles.",
        OperationId = "GetAllProfiles")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of profiles", typeof(IEnumerable<ProfileResource>))]
    public async Task<IActionResult> GetAllProfiles()
    {
        var profiles = await profileQueryService.Handle(new GetAllProfilesQuery());
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    [HttpGet("search")]
    [SwaggerOperation(
        Summary = "Search Profiles",
        Description = "Returns a list of profiles that match the search criteria.",
        OperationId = "SearchProfiles"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of profiles that match the search criteria", typeof(IEnumerable<ProfileResource>))]
    public async Task<IActionResult> SearchProfiles(
        [FromQuery] string? query,
        [FromQuery] string? role,
        [FromQuery] int? minScore,
        [FromQuery] int? maxScore,
        [FromQuery] bool excludeCurrentUser = true)
    {
        var searchQuery = new SearchProfilesQuery(query, role, minScore, maxScore, excludeCurrentUser);
        var profiles = await profileQueryService.Handle(searchQuery);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Profile",
        Description = "Creates a new profile and returns the created profile resource.",
        OperationId = "CreateProfile")]
    [SwaggerResponse(StatusCodes.Status201Created, "Profile created successfully", typeof(ProfileResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Profile could not be created")]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource resource)
    {
        try
        {
            var command = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(resource);
            var profile = await profileCommandService.Handle(command);
        
            if (profile is null) 
                return BadRequest("Profile could not be created. Possible reasons: UserId doesn't exist or username is already taken.");
        
            var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
            return CreatedAtAction(nameof(GetProfileById), new { id = profileResource.Id }, profileResource);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in CreateProfile endpoint: {ex.Message}");
            return BadRequest($"Profile could not be created: {ex.Message}");
        }
    }
    [HttpPatch("{id}/points")]
[SwaggerOperation(
    Summary = "Update Profile Points",
    Description = "Add or remove a point from a profile",
    OperationId = "UpdateProfilePoints"
)]
[SwaggerResponse(StatusCodes.Status200OK, "Points updated successfully", typeof(ProfileResource))]
[SwaggerResponse(StatusCodes.Status400BadRequest, "Points could not be updated")]
[SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found")]
public async Task<IActionResult> UpdateProfilePoints(
    [FromRoute] int id, 
    [FromBody] UpdateProfilePointsResource resource)
{
    try
    {
        var command = new UpdateProfilePointsCommand(
            id,
            resource.Points,
            resource.PointsGivenBy
        );

        var profile = await profileCommandService.Handle(command);
        
        if (profile is null) 
            return NotFound("Profile not found");
        
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in UpdateProfilePoints endpoint: {ex.Message}");
        return BadRequest($"Points could not be updated: {ex.Message}");
    }
}

// ENDPOINT ALTERNATivo para toggle de puntos
[HttpPost("{id}/points/toggle")]
[SwaggerOperation(
    Summary = "Toggle User Point",
    Description = "Add or remove a point from a user to a profile",
    OperationId = "ToggleUserPoint"
)]
[SwaggerResponse(StatusCodes.Status200OK, "Point toggled successfully", typeof(ProfileResource))]
[SwaggerResponse(StatusCodes.Status400BadRequest, "Point could not be toggled")]
[SwaggerResponse(StatusCodes.Status404NotFound, "Profile not found")]
public async Task<IActionResult> ToggleUserPoint(
    [FromRoute] int id, 
    [FromBody] ToggleUserPointResource resource)
{
    try
    {
        // Primero obtener el perfil actual
        var currentProfile = await profileQueryService.Handle(new GetProfileByIdQuery(id));
        if (currentProfile is null)
            return NotFound("Profile not found");

        // Hacer el toggle
        var pointsGivenBy = new List<string>(currentProfile.PointsGivenBy ?? new List<string>());
        
        if (pointsGivenBy.Contains(resource.UserId))
        {
            pointsGivenBy.Remove(resource.UserId);
        }
        else
        {
            pointsGivenBy.Add(resource.UserId);
        }

        var newPoints = pointsGivenBy.Count;

        var command = new UpdateProfilePointsCommand(
            id,
            newPoints,
            pointsGivenBy
        );

        var updatedProfile = await profileCommandService.Handle(command);
        
        if (updatedProfile is null) 
            return BadRequest("Point could not be toggled");
        
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(updatedProfile);
        return Ok(profileResource);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in ToggleUserPoint endpoint: {ex.Message}");
        return BadRequest($"Point could not be toggled: {ex.Message}");
    }
}
}