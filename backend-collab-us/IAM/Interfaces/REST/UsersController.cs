using System.Net.Mime;
using backend_collab_us.IAM.domain.model.queries;
using backend_collab_us.IAM.domain.Services;
using backend_collab_us.IAM.Interfaces.REST.Resources;
using backend_collab_us.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.IAM.Interfaces;

[ApiController]
[Route("api/v1/users")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Available Users Endpoints")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService) : ControllerBase
{
    [HttpGet("id:int")]
    [SwaggerOperation(
        Summary = "Get User by Id",
        Description = "Returns a user by its unique identifier.",
        OperationId = "GetUserById"
    )]
    [SwaggerResponse(StatusCodes.Status200OK, "User Found", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "User not found")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(id));
        if (user is null) return NotFound();
        var resource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return Ok(resource);
    }
    [HttpGet]
    [SwaggerOperation(
        Summary = "Get All Users",
        Description = "Returns a list of all users.",
        OperationId = "GetAllUsers")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of users", typeof(IEnumerable<UserResource>))]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await userQueryService.Handle(new GetAllUsersQuery());
        var resources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }    
    [HttpPost]
    [SwaggerOperation(
        Summary = "Create User",
        Description = "Creates a new user and returns the created user resource.",
        OperationId = "CreateUser")]
    [SwaggerResponse(StatusCodes.Status201Created, "User created successfully", typeof(UserResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "User could not be created")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserResource resource)
    {
        var command = CreateUserCommandFromResourceAssembler.ToCommandFromResource(resource);
        var user = await userCommandService.Handle(command);
        if (user is null) return BadRequest("User could not be created.");
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user);
        return new CreatedResult(string.Empty, userResource);
    }
}