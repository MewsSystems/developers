using System.Net.Http.Json;
using System.Text.Json;
using ExchangeRateUpdater.Dto;
using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.ExternalServices;

public class CnbExchangeRateClient(IOptions<CnbExchangeRateOptions> options, HttpClient httpClient, ILogger<CnbExchangeRateClient> logger) : ICnbExchangeRateClient
{
    private readonly string CommonUrl = options.Value.CommonUrl;
    private readonly string UncommonUrl = options.Value.UncommonUrl;
    public async Task<IEnumerable<ExchangeRate>> FetchCommonExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Fetching common CZK exchange rates from CNB");
        IEnumerable<ExchangeRate> exchangeRates = await FetchExchangeRatesAsync(CommonUrl, cancellationToken);
        return exchangeRates;
    }

    public async Task<IEnumerable<ExchangeRate>> FetchUncommonExchangeRatesAsync(CancellationToken cancellationToken = default)
    {
        DateTime targetMonth = DateTime.Now.AddMonths(-1);
        string url = $"{UncommonUrl}&yearMonth={targetMonth:yyyy-MM}";

        logger.LogInformation("Fetching uncommon CZK exchange rates from CNB");
        IEnumerable<ExchangeRate> exchangeRates = await FetchExchangeRatesAsync(url, cancellationToken);
        return exchangeRates;
    }
    internal async Task<IEnumerable<ExchangeRate>> FetchExchangeRatesAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            CnbExchangeRateResponseDto? exchangeRateDtos = await response.Content.ReadFromJsonAsync<CnbExchangeRateResponseDto>(cancellationToken: cancellationToken);

            if (exchangeRateDtos is null || exchangeRateDtos.Rates is null || !exchangeRateDtos.Rates.Any())
            {
                throw new NoExchangeRatesReceivedException(url);
            }

            IEnumerable<ExchangeRate> mappedExchangeRates = MapToExchangeRates(exchangeRateDtos.Rates);
            return mappedExchangeRates;
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(ex, "HTTP request failed when fetching rates from CNB. URL: {Url}", url);
            throw;
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Error deserializing response from CNB. URL: {Url}", url);
            throw;
        }
        catch (NoExchangeRatesReceivedException ex)
        {
            logger.LogError(ex, "Fetching from CNB returned no exchange rates. URL: {Url}", ex.Url);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while fetching rates from CNB. URL: {Url}", url);
            throw;
        }
    }

    internal IEnumerable<ExchangeRate> MapToExchangeRates(IEnumerable<CnbExchangeRate> cnbExchangeRateDtos)
    {
        Currency targetCurrency = new("CZK");
        IEnumerable<ExchangeRate> exchangeRates = [.. cnbExchangeRateDtos
        .Where(dto =>
        {
            if (dto.Rate is 0)
            {
                logger.LogWarning("Invalid Rate: {Rate}, for currency: {Currency}", dto.Rate, dto.Currency);
                return false;
            }
            return true;
        })
        .Select(dto =>
        {
            Currency sourceCurrency = new(dto.CurrencyCode);
            decimal normalizedRate = dto.Rate / dto.Amount;
            return new ExchangeRate(sourceCurrency, targetCurrency, normalizedRate);
        })];
        return exchangeRates;
    }
}
