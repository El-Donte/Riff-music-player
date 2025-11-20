using RiffBackend.API.Responses;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Middleware;
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            var responseError = new ResponseError("server.internal", ex.Message, default);
            var envelope = Envelope.Error([responseError]);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}