using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Application.Common.Behaviors
{
    /// <summary>
    /// Represents a logging behavior for logging information about command handling.
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <typeparam name="TResponse">The type of response produced.</typeparam>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="logger">The logger to be used for logging.</param>
        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling command: {CommandName}", typeof(TRequest).Name);
            var response = await next();
            _logger.LogInformation("Command handled: {CommandName}", typeof(TRequest).Name);

            return response;
        }
    }
}
