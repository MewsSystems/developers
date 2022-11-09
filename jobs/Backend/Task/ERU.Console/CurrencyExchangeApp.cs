using ERU.Application.Interfaces;
using ERU.Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ERU.Console
{
    internal class CurrencyExchangeApp : IHostedService
    {
        private readonly IExchangeRateProvider _currencyExchangeProvider;
        private readonly ILogger _logger;

        private static readonly IEnumerable<Currency> Currencies = new[] {
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

        public CurrencyExchangeApp(IExchangeRateProvider currencyExchangeProvider, ILogger<CurrencyExchangeApp> logger)
        {
            _currencyExchangeProvider = currencyExchangeProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Starting currency exchange app. Ctrl-C to end");

            await RetrieveAndOutputExchangeRates(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            _logger.Log(LogLevel.Information, "Stopping currency exchange app");
        }

        private async Task RetrieveAndOutputExchangeRates(CancellationToken cancellationToken)
        {
            try
            {
                CancellationTokenSource cancellationTokenSource = RegisterCancellationToken();
                List<ExchangeRate> rates = (await TryGetExchangeRates(_currencyExchangeProvider, Currencies, cancellationTokenSource.Token)).ToList();
                System.Console.WriteLine($"Successfully retrieved {rates.Count} exchange rates:");
                foreach (ExchangeRate rate in rates)
                    System.Console.WriteLine(rate.ToString());
            }
            catch (Exception e)
            {
                System.Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
            }

            System.Console.ReadLine();
        }

        private static async Task<IEnumerable<ExchangeRate>> TryGetExchangeRates(IExchangeRateProvider exchangeRateProvider, IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            try
            {
                return await exchangeRateProvider.GetExchangeRates(currencies, cancellationToken);
            }
            catch (AggregateException e) when (e.InnerExceptions.Any(ex => ex is OperationCanceledException or TaskCanceledException))
            {
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
            return new List<ExchangeRate>();
        }

        private static CancellationTokenSource RegisterCancellationToken()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            System.Console.CancelKeyPress += (s, e) =>
            {
                System.Console.WriteLine("Canceling...");
                cancellationTokenSource.Cancel();
                e.Cancel = true;
            };
            return cancellationTokenSource;
        }
    }
}
