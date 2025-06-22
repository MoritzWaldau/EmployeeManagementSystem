using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors.Validation;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationPipelineBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = 
            await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        var failures = 
            validationResults
                .Where(x => x.Errors.Count != 0)
                .SelectMany(x => x.Errors)
                .ToList();

        if (failures.Count != 0)
        {
            logger.LogWarning("Validation failed for request {RequestName}: {Errors}", 
                typeof(TRequest).Name, string.Join(", ", failures.Select(f => f.ErrorMessage)));
            throw new ValidationException(failures);
        }
            
        
        return await next(cancellationToken);
    }
}