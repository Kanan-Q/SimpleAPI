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

    // Xətanın JSON formatında HTTP cavabına çevrilməsi üçün köməkçi metod
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Cavabın tipi JSON olaraq təyin edilir
        context.Response.ContentType = "application/json";

        // Xətanın növünə görə HTTP status kodunu təyin edirik
        var statusCode = exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,  // 400
            KeyNotFoundException => StatusCodes.Status404NotFound,     // 404
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized, // 401
            _ => StatusCodes.Status500InternalServerError              // 500 (digər xətalar üçün)
        };

        context.Response.StatusCode = statusCode;

        // JSON formatında cavabın strukturu
        var response = new
        {
            StatusCode = statusCode,
            Message = exception.Message,
            // Detail = exception.StackTrace  // Debug üçün aktivləşdirilə bilər
        };

        // JSON-a çevrilir
        var json = JsonSerializer.Serialize(response);

        // HTTP cavabı kimi göndərilir
        return context.Response.WriteAsync(json);
    }
}
