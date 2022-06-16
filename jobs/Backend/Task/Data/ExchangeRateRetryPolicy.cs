namespace ExchangeRateUpdater.Data
{
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Domain;
    using Polly;
    using System;
    using System.Net.Http;

    public class ExchangeRateRetryPolicy : IRetryPolicy<BankDetails>
    {
        private readonly ILogger logger;

        public ExchangeRateRetryPolicy(ILogger logger)
        {
            this.logger = logger;
        }

        public BankDetails ExecuteWithRetry(Func<BankDetails> action) => 
            Policy.Handle<HttpRequestException>(ex => ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .WaitAndRetry(
                    retryCount: 3,
                    sleepDurationProvider: (attemptNumber) => TimeSpan.FromSeconds(5),
                    onRetry: (exception, sleepDuration, attemptNumber, context) =>
                    {
                        logger.LogWarning($"Too many request currently retrying in {sleepDuration}. {attemptNumber}s so far.");
                    })
                .Execute(action);
    }
}
