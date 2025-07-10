namespace API.Middleware;

public class ExceptionMiddleware(ILogger<ExceptionMiddleware> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("{Time} Error : {Message}", exception.Message, DateTime.UtcNow);

        (string Message, string Title, int StatusCode) details = exception switch
        {
            _ => 
            (
                exception.Message,
                exception.GetType().Name,
                context.Response.StatusCode = StatusCodes.Status500InternalServerError
            )
        };
        
        var problemDetails = new ProblemDetails
        {
            Title = details.Title,
            Detail = details.Message,
            Status = details.StatusCode,
            Instance = context.Request.Path
        };
        
        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);
        return true;
    }
}