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
            var timeTakenInMilliseconds = timer.Elapsed.Milliseconds;
            if (timeTakenInMilliseconds > 3000)
            {
                logger.LogWarning("[PERFORMANCE] The request {Request} took {TimeTaken}",
                    typeof(TRequest).Name, timeTakenInMilliseconds);
            }

            logger.LogInformation("[END] Hanlded {Request} with {Respose} processed in {Milliseconds} milliseconds",
                typeof(TRequest).Name, typeof(TResponse).Name, timeTakenInMilliseconds);

            return response;
        }
    }
}
