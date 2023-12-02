using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Ports;
using ExchangeRateUpdater.Domain.ValueObjects;
using Flurl;
using Serilog;
using System;
using System.Linq;

namespace Adapter.ExchangeRateProvider.CzechNationalBank;

public class CzechNationalBankRepository : IExchangeRateProviderRepository
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger _logger;

    public CzechNationalBankRepository(IHttpClientFactory? httpClientFactory, ILogger? logger)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<ExchangeRate>> GetDefaultUnitRates()
    {
        var httpClient = CreateClient();

        var response = await httpClient.GetAsync(GetExchangeAsTextUrl(DateTime.Now));

        using var contentStream = await response.Content.ReadAsStreamAsync();

        using var exchangeRatesTextParser = new ExchangeRatesTextParser(new StreamReader(contentStream), _logger);


        var rawData = await exchangeRatesTextParser.GetDefaultFormattedExchangeRatesAsync();

        return rawData.Select(dto => {
            var targetCurrency = new Currency("CZK");
            var sourceCurrency = new Currency(dto.CurrencyCode);
            // In case amount is 100 or something else.
            var rate = new PositiveRealNumber(dto.Rate / dto.Amount);
            return new ExchangeRate(sourceCurrency, targetCurrency, rate);
        });
    }

    public Task<ExchangeRate?> GetExchangeRateForCurrenciesAsync(Currency sourceCurrency, Currency targetCurrency)
    {
        throw new NotImplementedException();
    }

    


    private HttpClient CreateClient()
    {
        return _httpClientFactory.CreateClient("ExchangeRateUpdater-http-client");
    }

    private Url GetExchangeAsTextUrl(DateTime date)
    {
        return "financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt".SetQueryParam("date", date.Date);
    }
}
