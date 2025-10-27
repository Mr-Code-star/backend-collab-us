using System.Net.Mime;
using backend_collab_us.profile_managment.domain.model.queries;
using backend_collab_us.profile_managment.domain.services;
using backend_collab_us.profile_managment.Interfaces.REST.Resources;
using backend_collab_us.profile_managment.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.profile_managment.Interfaces.REST;

[ApiController]
[Route("api/v1/comments")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available comment Endpoints")]
public class CommentController(
    ICommentCommandService commentCommandService,
    ICommentQueryService commentQueryService) : ControllerBase
{
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get Comments by Profile Id",
        Description = "Returns all comments for a specific profile.",
        OperationId = "GetCommentsByProfileId"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "Comments Found", typeof(IEnumerable<CommentResource>))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "No comments found")]
    public async Task<IActionResult> GetCommentsByProfileId([FromQuery] int profileId)
    {
        var comments = await commentQueryService.Handle(new GetCommentsByProfileIdQuery(profileId));
        if (!comments.Any()) return NotFound("No comments found for this profile");
        var resources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create Comment",
        Description = "Creates a new comment and returns the created comment resource.",
        OperationId = "CreateComment")]
    [SwaggerResponse(StatusCodes.Status201Created, "Comment created successfully", typeof(CommentResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Comment could not be created")]
    public async Task<IActionResult> CreateComment([FromBody] CreateCommentResource resource)
    {
        try
        {
            var command = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var comment = await commentCommandService.Handle(command);
    
            if (comment is null) 
                return BadRequest("Comment could not be created. Possible reasons: ProfileId or UserId doesn't exist.");
    
            var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
            return CreatedAtAction(nameof(GetCommentsByProfileId), new { profileId = commentResource.ProfileId }, commentResource);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in CreateComment endpoint: {ex.Message}");
            return BadRequest($"Comment could not be created: {ex.Message}");
        }
    }
}