using MediatR;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Behaviors;

/// <summary>
/// Pipeline behavior that catches and logs unhandled exceptions.
/// This should be the outermost behavior in the pipeline.
/// </summary>
public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Common.Exceptions.ValidationException ex)
        {
            // ValidationException is expected - log as warning, not error
            var requestName = typeof(TRequest).Name;
            _logger.LogWarning(
                "Validation failed for request {RequestName}: {Errors}",
                requestName,
                string.Join(", ", ex.Errors.SelectMany(e => e.Value)));
            throw;
        }
        catch (Common.Exceptions.NotFoundException ex)
        {
            // NotFoundException is expected - log as information
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation(
                "Resource not found for request {RequestName}: {Message}",
                requestName,
                ex.Message);
            throw;
        }
        catch (Common.Exceptions.ApplicationException ex)
        {
            // Other application exceptions are expected business rule violations
            var requestName = typeof(TRequest).Name;
            _logger.LogWarning(
                ex,
                "Application exception for request {RequestName}: {Message}",
                requestName,
                ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            // Truly unexpected exceptions - log as error
            var requestName = typeof(TRequest).Name;
            _logger.LogError(
                ex,
                "Unhandled exception for request {RequestName}: {ExceptionMessage}",
                requestName,
                ex.Message);
            throw;
        }
    }
}
