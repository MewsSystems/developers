namespace ExchangeRateProvider.Infrastructure.Interfaces;

public interface IHttpRetryPolicy
{
    AsyncPolicy<HttpResponseMessage> CNBHttpPolicy { get; }
}