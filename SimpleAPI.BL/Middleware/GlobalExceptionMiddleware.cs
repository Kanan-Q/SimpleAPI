using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> looger) => (_next, _logger) = (next, looger);


    // Middleware-nin əsas metodu: hər HTTP request burada keçir
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "error");
            await HandleExceptionAsync(httpContext, ex);
        }

    }

    private static Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Request.ContentType = "application/json";

        var statusCode = ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };
        context.Response.StatusCode = statusCode;

        var response = new
        {
            StatusCode = statusCode,
            Message = ex.Message
        };
        var json = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(json);
    }

}
