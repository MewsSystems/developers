using ExchangeRateUpdater.BankRatesManagers;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider: IExchangeRateProvider
{
    private readonly HttpClient httpClient;
    private readonly IBankRatesManager bankRatesManager;

    public ExchangeRateProvider(HttpClient httpClient, IBankRatesManager bankRatesManager)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.bankRatesManager = bankRatesManager ?? throw new ArgumentNullException(nameof(bankRatesManager));
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var rates = await httpClient.GetStringAsync(bankRatesManager.GetDailyDataSourceUri());
        return bankRatesManager.Parse(rates)
            .Where(line => currencies.Contains(line.TargetCurrency));
    }
}
