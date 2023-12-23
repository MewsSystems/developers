using ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;
using Refit;

namespace ExchangeRateUpdater.DependencyInjection.CnbApi;

public static class CnbApiClientInjector
{
    public static ICnbApiClient GetCnbApiClient(string hostUrl)
    {
        var options = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
        options.Converters.Add(new IsoDateOnlyConverter());
        
        return RestService.For<ICnbApiClient>(hostUrl, new RefitSettings()
        {
            UrlParameterFormatter = new IsoDateOnlyUrlParameterFormatter(),
            ContentSerializer = new SystemTextJsonContentSerializer(options)
        });
    }
}