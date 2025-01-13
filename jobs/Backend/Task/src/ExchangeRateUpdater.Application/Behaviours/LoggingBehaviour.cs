using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ExchangeRateUpdater.Application.Behaviours
{
    public class LoggingBehaviour<TRequest, TResponse>
        (ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull, IRequest<TResponse>
        where TResponse : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            logger.LogInformation("[START] Handle request={Request} - Response={Respose} - RequestData={RequestData}",
                typeof(TRequest).Name, typeof(TResponse).Name, request);

            var timer = new Stopwatch();
            timer.Start();

            var response = await next();

            timer.Stop();
            var timeTakenInSeconds = timer.Elapsed.Seconds;
            if (timeTakenInSeconds > 3)
            {
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                    typeof(TRequest).Name, timeTakenInSeconds);
            }

            logger.LogInformation("[END] Hanlded {Request} with {Respose} processed in {Seconds} seconds",
                typeof(TRequest).Name, typeof(TResponse).Name, timeTakenInSeconds);

            return response;
        }
    }
}
