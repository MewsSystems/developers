using Flurl;
using Polly;
using Serilog;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Unit;

internal class CzechNationalBankRepositoryTestDouble : CzechNationalBankRepository
{
    public int ExecutionCounter = 0;
    public CzechNationalBankRepositoryTestDouble(IHttpClientFactory? httpClientFactory, ILogger? logger) : base(httpClientFactory, logger)
    {
    }

    protected override void OnRetry(Exception exception, Context context)
    {
        ++ExecutionCounter;
        base.OnRetry(exception, context);
    }

    protected override TimeSpan[] GetRetrySleepTimes()
    {
        return new TimeSpan[]
        {
            TimeSpan.FromMicroseconds(1),
            TimeSpan.FromMicroseconds(1)
        };
    }

    // Looks Wiremock for some reason couldn't handle the original Url.
    protected override Url GetAllExchangeRatesAsTextUrl(DateTime date)
    {
        return Url.Parse("Test");
    }
}
