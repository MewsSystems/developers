using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.ConsoleApp.Dto;
using ExchangeRateProvider.Domain.Entities;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateProvider.ConsoleApp;

/// <summary>
/// Background service that retrieves and displays exchange rates for predefined currencies,
/// then stops the application.
/// </summary>
/// <param name="exchangeRateProvider">Service for retrieving exchange rates.</param>
/// <param name="lifetime">Host application lifetime for controlling application shutdown.</param>
public class ExchangeRateUpdaterWorker(
    IExchangeRateProviderService exchangeRateProvider,
    IHostApplicationLifetime lifetime)
    : BackgroundService
{
    /// <summary>
    /// Executes the background service by retrieving exchange rates for a predefined set of
    /// currencies and displaying them to the console.
    /// </summary>
    /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            var currencies = new[]
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

            var exchangeRates = (await exchangeRateProvider
                .GetLatestAsync(currencies, cancellationToken)).ToList();

            var exchangeRatesDto = exchangeRates
                .Select(r => new ExchangeRateDto(r.SourceCurrency, r.TargetCurrency, r.Value))
                .ToList();

            Console.WriteLine($"Successfully retrieved {exchangeRatesDto.Count} exchange rates:");
            exchangeRatesDto.ForEach(Console.WriteLine);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{ex.Message}'.");
            Environment.ExitCode = 1;
        }
        finally
        {
            lifetime.StopApplication();

            Console.ReadLine();
        }
    }
}
