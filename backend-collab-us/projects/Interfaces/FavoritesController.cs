using System.Net.Mime;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.services;
using backend_collab_us.projects.Interfaces.REST.Resources;
using backend_collab_us.projects.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.projects.Interfaces;

[ApiController]
[Route("api/v1/favorites")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Favorites Endpoints")]
public class FavoritesController(
    IFavoriteCommandService favoriteCommandService,
    IFavoriteQueryService favoriteQueryService) : ControllerBase
{
    [HttpGet("profile/{profileId}")]
    [SwaggerOperation(
        Summary = "Get Favorites by Profile",
        Description = "Returns all favorite projects for a specific profile.",
        OperationId = "GetFavoritesByProfile"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of favorite projects", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> GetFavoritesByProfile([FromRoute] int profileId)
    {
        var projects = await favoriteQueryService.Handle(new GetFavoriteProjectsByProfileIdQuery(profileId));
        var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("profile/{profileId}/check/{projectId}")]
    [SwaggerOperation(
        Summary = "Check if Project is Favorite",
        Description = "Checks if a project is marked as favorite by a profile.",
        OperationId = "CheckIfProjectIsFavorite"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Favorite status", typeof(bool))]
    public async Task<IActionResult> CheckIfProjectIsFavorite(
        [FromRoute] int profileId,
        [FromRoute] int projectId)
    {
        var isFavorite = await favoriteQueryService.Handle(new CheckIfProjectIsFavoriteQuery(profileId, projectId));
        return Ok(new { isFavorite });
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Add to Favorites",
        Description = "Adds a project to profile favorites.",
        OperationId = "AddToFavorites")]
    [SwaggerResponse(StatusCodes.Status201Created, "Added to favorites successfully", typeof(FavoriteResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Could not add to favorites")]
    public async Task<IActionResult> AddToFavorites([FromBody] CreateFavoriteResource resource)
    {
        try
        {
            var command = CreateFavoriteCommandFromResourceAssembler.ToCommandFromResource(resource);
            var favorite = await favoriteCommandService.Handle(command);
        
            if (favorite is null) 
                return BadRequest("Could not add to favorites. Possible reasons: Project doesn't exist or already in favorites.");
        
            var favoriteResource = FavoriteResourceFromEntityAssembler.ToResourceFromEntity(favorite);
            return CreatedAtAction(nameof(GetFavoritesByProfile), new { profileId = favoriteResource.ProfileId }, favoriteResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in AddToFavorites endpoint: {ex.Message}");
            return BadRequest($"Could not add to favorites: {ex.Message}");
        }
    }
}