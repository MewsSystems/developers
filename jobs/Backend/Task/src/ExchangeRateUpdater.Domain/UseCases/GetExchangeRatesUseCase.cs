using Domain.Entities;
using Domain.Ports;
using Serilog;

namespace Domain.UseCases;

public class GetExchangeRatesUseCase
{
    private readonly IExchangeRatesSearcher _exchangeRatesSearcherService;
    private readonly ILogger _logger;
    private readonly bool _useInMemoryCache;
    private readonly IExchangeRateInMemoryCache _cacheHelper;

    private const string _exchangeRateCacheKey = "DailyExchangeRates";

    private static readonly IEnumerable<Currency> _currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };

    public GetExchangeRatesUseCase(
        IExchangeRatesSearcher exchangeRatesSearcherService,
        ILogger logger,
        bool useInMemoryCache,
        IExchangeRateInMemoryCache cacheHelper)
    {
        _exchangeRatesSearcherService = exchangeRatesSearcherService ?? throw new ArgumentNullException(nameof(exchangeRatesSearcherService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _useInMemoryCache = useInMemoryCache;
        _cacheHelper = cacheHelper;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            IEnumerable<ExchangeRate> exchangeRates = null;
            
            if (_useInMemoryCache)
            {
                if (_cacheHelper != null)
                {
                    exchangeRates = _cacheHelper.GetCache(_exchangeRateCacheKey);

                    if (exchangeRates == null)
                    {
                        _logger.Information($"Successfully retrieved {exchangeRates?.Count()} exchange rates");

                        exchangeRates = await GetExchangeRatesFromCzechBankApi();
                        _cacheHelper.SetCache(_exchangeRateCacheKey, exchangeRates);
                    }
                    else
                    {
                        _logger.Information($"Successfully retrieved {exchangeRates?.Count()} exchange rates from in memory cache");
                    }
                }
            }
            else
            {
                exchangeRates = await GetExchangeRatesFromCzechBankApi();
            }
            
            var filteredExchangeRates = exchangeRates?
                .Where(r => _currencies.Any(c => c.Code == r.SourceCurrency.Code));

            var filteredRecordsCount = exchangeRates?.Count() - filteredExchangeRates?.Count();
            _logger.Information($"Filtered {filteredRecordsCount} exchange rates from the retrieved list");

            foreach (var rate in filteredExchangeRates)
            {
                _logger.Information(rate.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
        }
    }

    private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesFromCzechBankApi()
    {
        var exchangeRates = await _exchangeRatesSearcherService.GetExchangeRates(DateTime.Now);
        _logger.Information($"Successfully retrieved {exchangeRates?.Count()} exchange rates from API");
        return exchangeRates;
    }
}