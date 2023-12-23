using ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;
using ExchangeRateUpdater.Model.ExchangeRates;
using Refit;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates;

public class ExchangeRateDataSource(
    ICnbApiClient cnbApiClient,
    IExchangeRateMapper exchangeRateMapper,
    ITimeService timeService) : IExchangeRateDataSource
{
    private const string CzechLanguage = "CZ";
    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(CancellationToken cancellationToken)
    {
        var currentDate = DateOnly.FromDateTime(timeService.GetCurrentTime());
        
        CnbExchangeRatesDailyResponse? response;
        try
        {
            response = await cnbApiClient.GetDailyExchangeRatesAsync(currentDate, CzechLanguage, cancellationToken);
        }
        catch (ApiException)
        {
            // API request failed - here I would log the exception before returning an empty value.
            return Enumerable.Empty<ExchangeRate>();
        }

        return exchangeRateMapper.MapFromCnbExchangeRatesDailyResponse(response);
    }
}