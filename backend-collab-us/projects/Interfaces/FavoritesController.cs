using System.Net.Mime;
using backend_collab_us.projects.domain.model.queries;
using backend_collab_us.projects.domain.repository;
using backend_collab_us.projects.domain.services;
using backend_collab_us.projects.Interfaces.REST.Resources;
using backend_collab_us.projects.Interfaces.REST.Transform;
using backend_collab_us.Shared.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.projects.Interfaces;

[ApiController]
[Route("api/v1/favorites")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Favorites Endpoints")]
public class FavoritesController(
    IFavoriteCommandService favoriteCommandService,
    IFavoriteQueryService favoriteQueryService,
    IFavoriteRepository favoriteRepository,
    IUnitOfWork unitOfWork) : ControllerBase
{
    [HttpGet("profile/{profileId}")]
    [SwaggerOperation(
        Summary = "Get Favorites by Profile",
        Description = "Returns all favorite projects for a specific profile.",
        OperationId = "GetFavoritesByProfile"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of favorite projects", typeof(IEnumerable<FavoriteResource>))]
    public async Task<IActionResult> GetFavoritesByProfile([FromRoute] int profileId)
    {
        try
        {
            var favorites = await favoriteQueryService.Handle(new GetFavoritesByProfileIdQuery(profileId));
            var resources = favorites.Select(FavoriteResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting favorites: {ex.Message}");
            return StatusCode(500, "Error retrieving favorites");
        }
    }

    [HttpGet("profile/{profileId}/projects")]
    [SwaggerOperation(
        Summary = "Get Favorite Projects by Profile",
        Description = "Returns all favorite projects for a specific profile.",
        OperationId = "GetFavoriteProjectsByProfile"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "List of favorite projects", typeof(IEnumerable<ProjectResource>))]
    public async Task<IActionResult> GetFavoriteProjectsByProfile([FromRoute] int profileId)
    {
        try
        {
            var projects = await favoriteQueryService.Handle(new GetFavoriteProjectsByProfileIdQuery(profileId));
            var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting favorite projects: {ex.Message}");
            return StatusCode(500, "Error retrieving favorite projects");
        }
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
        try
        {
            var isFavorite = await favoriteQueryService.Handle(new CheckIfProjectIsFavoriteQuery(profileId, projectId));
            return Ok(new { isFavorite });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking favorite: {ex.Message}");
            return StatusCode(500, "Error checking favorite status");
        }
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

    [HttpDelete("profile/{profileId}/project/{projectId}")]
    [SwaggerOperation(
        Summary = "Remove from Favorites",
        Description = "Removes a project from profile favorites.",
        OperationId = "RemoveFromFavorites")]
    [SwaggerResponse(StatusCodes.Status200OK, "Removed from favorites successfully")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Favorite not found")]
    public async Task<IActionResult> RemoveFromFavorites([FromRoute] int profileId, [FromRoute] int projectId)
    {
        try
        {
            // Buscar el favorito por profileId y projectId
            var favorite = await favoriteRepository.FindByProfileAndProjectAsync(profileId, projectId);
            if (favorite == null)
                return NotFound("Favorite not found");

            // Eliminar el favorito
            favoriteRepository.Remove(favorite);
            await unitOfWork.CompleteAsync();

            return Ok(new { message = "Removed from favorites successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in RemoveFromFavorites endpoint: {ex.Message}");
            return BadRequest($"Could not remove from favorites: {ex.Message}");
        }
    }
}