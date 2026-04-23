using ExchangeRateUpdater.Domain.Entities;
using ExchangeRateUpdater.Domain.Interfaces;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Clients;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Exceptions;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Parsers;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank;

/// <summary>
/// Exchange rate provider backed by the Czech National Bank daily rates feed.
/// </summary>
/// <remarks>
/// This provider fetches and parses daily exchange rates published by the CNB.
/// </remarks>
internal class ExchangeRateProvider : IExchangeRateProvider
{
    private static readonly Currency _targetCurrency = new("CZK");

    private readonly ICzechNationalBankClient _client;
    private readonly IDailyExchangeRatesResponseParser _parser;
    private readonly ILogger<ExchangeRateProvider> _logger;

    public ExchangeRateProvider(
        ICzechNationalBankClient client,
        IDailyExchangeRatesResponseParser parser,
        ILogger<ExchangeRateProvider> logger)
    {
        _client = client;
        _parser = parser;
        _logger = logger;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
        IEnumerable<Currency> currencies,
        CancellationToken cancellationToken = default)
    {
        var currencyList = currencies.ToList();
        _logger.LogInformation(
            "Fetching exchange rates for {Count} currencies: {Currencies}",
            currencyList.Count,
            string.Join(", ", currencyList.Select(c => c.Code))
        );

        try
        {
            var rawData = await _client.GetDailyRatesAsync(cancellationToken);
            var data = _parser.Parse(rawData);

            var requestedCodes = new HashSet<string>(
                currencyList.Select(c => c.Code),
                StringComparer.OrdinalIgnoreCase
            );

            var exchangeRates = data
                .ExchangeRates
                .Where(model => requestedCodes.Contains(model.Code))
                .Select(DomainExchangeRate)
                .ToList();

            _logger.LogInformation(
                "Successfully retrieved {Count} exchange rates out of {Requested} requested",
                exchangeRates.Count,
                currencyList.Count
            );

            LogNotFoundCurrencies(currencyList, exchangeRates);

            return exchangeRates;
        }
        catch (CzechNationalBankParsingException ex)
        {
            _logger.LogError(ex, "Failed to parse exchange rates from Czech National Bank response");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve exchange rates");
            throw;
        }
    }

    private void LogNotFoundCurrencies(List<Currency> currencyList, List<ExchangeRate> exchangeRates)
    {
        var foundCodes = new HashSet<string>(
            exchangeRates.Select(rate => rate.SourceCurrency.Code),
            StringComparer.OrdinalIgnoreCase
        );

        var missingCurrencies = currencyList
            .Where(rate => !foundCodes.Contains(rate.Code))
            .Select(rate => rate.Code)
            .ToList();

        if (missingCurrencies.Any())
        {
            _logger.LogWarning(
                "Could not find exchange rates for currencies: {Currencies}",
                string.Join(", ", missingCurrencies)
            );
        }
    }

    private static ExchangeRate DomainExchangeRate(Models.ExchangeRate model) =>
        new(
            sourceCurrency: new Currency(model.Code),
            targetCurrency: _targetCurrency,
            value: model.Rate / model.Amount
        );
}