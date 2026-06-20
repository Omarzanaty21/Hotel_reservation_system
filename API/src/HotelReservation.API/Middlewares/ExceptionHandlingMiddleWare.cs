
using System.Net;
using System.Text.Json;
using Application.DTOs.Common;

namespace HotelReservation.API.Middlewares;
    
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var statusCode = HttpStatusCode.InternalServerError;
        var errorCode = "SERVER_ERROR";
        var message = "A server error occurred. Please try again later.";

        // Map your application exceptions to HTTP statuses
        if (exception is KeyNotFoundException)
        {
            statusCode = HttpStatusCode.NotFound;
            errorCode = "NOT_FOUND";
            message = exception.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            statusCode = HttpStatusCode.Unauthorized;
            errorCode = "UNAUTHORIZED";
            message = exception.Message;
        }

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse(message, errorCode);
        
        // Force camelCase layout so JavaScript reads it natively as response.message
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var jsonResponse = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(jsonResponse);
    }
}
