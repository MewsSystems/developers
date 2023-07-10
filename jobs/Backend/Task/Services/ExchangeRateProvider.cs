using ExchangeRateUpdater.ApiClients.CzechNationalBank;
using ExchangeRateUpdater.ApiClients.Responses;
using ExchangeRateUpdater.Mappings;
using ExchangeRateUpdater.Models.Errors;
using ExchangeRateUpdater.Models.Time;
using ExchangeRateUpdater.Models.Types;
using ExchangeRateUpdater.Utilities;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services;

internal class ExchangeRateProvider : IExchangeRateProvider
{
    private const string TargetCurrency = "CZK";

    private readonly IExchangeRateApiClient _exchangeRateApiClient;

    public ExchangeRateProvider(IExchangeRateApiClient exchangeRateApiClient)
    {
        _exchangeRateApiClient = exchangeRateApiClient;
    }

    public async Task<OneOf<IEnumerable<ExchangeRate>, Error>> GetExchangeRates(IEnumerable<Currency> sourceCurrencies)
    {
        if (!sourceCurrencies.IsAny())
            return new Error(errorType: ErrorType.ValidationError)
                .WithMessage("Source currencies list not provided while getting exchange rates.");

        var apiDailyResponseTask = _exchangeRateApiClient.GetDaily();
        var apiOtherResponseTask = _exchangeRateApiClient.GetOtherByYearMonth(DateTime.UtcNow.GetYearMonthString());

        await Task.WhenAll(apiDailyResponseTask, apiOtherResponseTask);

        var apiDailyResponse = await apiDailyResponseTask;
        var apiOtherResponse = await apiOtherResponseTask;

        if (!apiDailyResponse.IsSuccessStatusCode)
            return new Error(errorType: ErrorType.ApiError)
                .WithMessage($"{apiDailyResponse.GetEndpointUrl()} failed with message: {apiDailyResponse.Error.Message}.");

        if (!apiOtherResponse.IsSuccessStatusCode)
            return new Error(errorType: ErrorType.ApiError)
                .WithMessage($"{apiDailyResponse.GetEndpointUrl()} failed with message: {apiOtherResponse.Error.Message}.");

        return apiDailyResponse.Content.Rates
            .Concat(apiOtherResponse.Content.Rates)
            .Where(IsCurrencyApiItemAmongSourceCurrencies(sourceCurrencies))
            .Select(exchangeRateApiItem => exchangeRateApiItem.ToExchangeRateResult(TargetCurrency))
            .ToList();
    }

    private static Func<ExchangeRateApiItem, bool> IsCurrencyApiItemAmongSourceCurrencies(IEnumerable<Currency> sourceCurrencies) =>
        exchangeRateApiItem => sourceCurrencies.Any(IsMatch(exchangeRateApiItem));

    private static Func<Currency, bool> IsMatch(ExchangeRateApiItem exchangeRateApiItem) =>
        sourceCurrency => sourceCurrency.Code.Value == exchangeRateApiItem.CurrencyCode;
}
