using System.Net;
using System.Text.Json;
namespace backend_collab_us.Shared.Domain.Exeptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            GeneralException => (int)HttpStatusCode.BadRequest,
            ArgumentException => (int)HttpStatusCode.BadRequest,
            InvalidOperationException => (int)HttpStatusCode.Conflict,
            _ => (int)HttpStatusCode.InternalServerError
        };

        var result = exception switch
        {
            GeneralException ge => JsonSerializer.Serialize(new { error = ge.Message, code = ge.Code }),
            ArgumentException => JsonSerializer.Serialize(new { error = exception.Message, code = "ARGUMENT_ERROR" }),
            InvalidOperationException => JsonSerializer.Serialize(new { error = exception.Message, code = "ALREADY_EXISTS" }),
            _ => JsonSerializer.Serialize(new { error = "Ocurrió un error inesperado.", code = "INTERNAL_ERROR" })

        };

        return context.Response.WriteAsync(result);
    }
}