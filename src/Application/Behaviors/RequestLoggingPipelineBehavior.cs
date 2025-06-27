namespace Application.Behaviors;

public sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
    (ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //Optional request logging
        /*logger.LogInformation("[START] Handle request={Request}",
            typeof(TRequest).Name);
            */

        var timer = new Stopwatch();
        timer.Start();
        var response = await next(cancellationToken);
        
        timer.Stop();

        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
            logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                typeof(TRequest).Name, timeTaken.Seconds);
        
        //Optional request logging
        /*logger.LogInformation("[END] Handled {Request}",
            typeof(TRequest).Name);
            */

        return response;
    }
}