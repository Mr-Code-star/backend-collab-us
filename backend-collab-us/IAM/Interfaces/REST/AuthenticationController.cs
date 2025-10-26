using System.Net.Mime;
using backend_collab_us.IAM.domain.Services;
using backend_collab_us.IAM.Interfaces.REST.Resources;
using backend_collab_us.IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace backend_collab_us.IAM.Interfaces.REST;
[ApiController]
[Route("api/v1/authentication")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Authentication endpoint")]

public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{
    [HttpPost("sign-in")]
    [SwaggerOperation(Summary = "Sign in", Description = "Authenticate a user", OperationId = "SignIn")]
    [SwaggerResponse(StatusCodes.Status200OK, "The user was authenticated", typeof(AuthenticatedUserResource))]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Invalid credentials", typeof(string))]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        try
        {
            var command = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
            var result = await userCommandService.Handle(command);
            var resource = AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(result.user, result.token);
            return Ok(resource);
        }
        catch (Exception e)
        {
            return Unauthorized(e.Message);
        }
    }
}