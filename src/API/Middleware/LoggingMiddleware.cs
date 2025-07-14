namespace API.Middleware;

public class LoggingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var start = Stopwatch.GetTimestamp();
        
        context.Request.EnableBuffering();
        var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
        context.Request.Body.Position = 0;

        var originalResponseBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await next(context);
        var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        var logLevel = context.Response.StatusCode >= 400 ? LogEventLevel.Warning : LogEventLevel.Information;
        Log.Write(logLevel, "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms.", 
            context.Request.Method, 
            context.Request.Path.Value, 
            context.Response.StatusCode,
            elapsedMs);

        await responseBody.CopyToAsync(originalResponseBodyStream);
    }
    
    private static double GetElapsedMilliseconds(long start, long stop)
    {
        return (stop - start) * 1000 / (double)Stopwatch.Frequency;
    }

}