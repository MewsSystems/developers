using Microsoft.Extensions.Logging;
using Services.CzechCrown.Models;

namespace Services.CzechCrown;

internal class CzechCrownRateProvider : IExchangeRateProvider
{
    private readonly ICzechNationalBankClient _czechNationalBankClient;
    private readonly ILogger<CzechCrownRateProvider> _logger;

    public CzechCrownRateProvider(ICzechNationalBankClient czechNationalBankClient, ILogger<CzechCrownRateProvider> logger)
    {
        _czechNationalBankClient = czechNationalBankClient;
        _logger = logger;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken ct = default)
    {
        var currencyList = currencies.Distinct().ToList();
        var executionDate = DateOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogInformation("Getting exchange rates for {CurrencyCount} currencies (after deduplication)", currencyList.Count);
        _logger.LogDebug("Getting exchange rates for {Currencies}", string.Join(", ", currencyList.Select(c => c.Code)));

        var clientResponse = await _czechNationalBankClient.GetExchangeRates(executionDate, ct);
        var primaryResults = GetRatesFromClientResponse(currencyList, clientResponse, executionDate);

        if (primaryResults.Count == currencyList.Count)
        {
            _logger.LogInformation("Execution complete");
            _logger.LogDebug("All requested currencies found in primary response");
            return primaryResults.Order();
        }

        if (ct.IsCancellationRequested)
        {
            _logger.LogDebug("Operation cancelled, returning empty list");
            return [];
        }

        var remainingCurrencies = currencyList.Except(primaryResults.Select(r => r.SourceCurrency)).ToList();

        var secondaryClientResponse = await _czechNationalBankClient.GetOtherExchangeRates(executionDate, ct);
        var secondaryResults = GetRatesFromClientResponse(remainingCurrencies, secondaryClientResponse, executionDate);

        var results = primaryResults.Concat(secondaryResults).Order();

        _logger.LogInformation("Execution complete");
        _logger.LogDebug("Found {ResultCount} exchange rates in total for {CurrencyCount} requested currencies", results.Count(), currencyList.Count);
        
        return results;
    }

    private List<ExchangeRate> GetRatesFromClientResponse(IEnumerable<Currency> currencies, CzkExchangeRateResponse clientResponse, DateOnly date)
    {
        List<ExchangeRate> czechCrownExchangeRates = [];
        foreach (var currency in currencies)
        {
            var rate = clientResponse.Rates.Where(r => string.Equals(r.CurrencyCode, currency.Code, StringComparison.OrdinalIgnoreCase))
                .MaxBy(r => r.ValidFor);
            if (rate == null)
            {
                _logger.LogDebug("Currency {Currency} not found in response", currency.Code);
                continue;
            }

            if (rate.ValidFor != date || rate.ValidFor > date)
            {
                _logger.LogWarning("Rate for {Currency} is not for the requested date {Date}, using latest available rate (from {RateDate})", currency.Code, date, rate.ValidFor);
            }

            var rateValue = rate.Rate / rate.Amount;
            _logger.LogDebug("Found exchange rate of {Date} for {Currency}: {RateValue}", rate.ValidFor, currency.Code, rateValue);
            czechCrownExchangeRates.Add(new ExchangeRate(new Currency(currency.Code), Currency.Czk, rateValue));
        }
        return czechCrownExchangeRates;
    }
}