using ExchangeRateProviderService;
using ExchangeRateProviderService.CNBExchangeRateProviderService;
using ExchangeRateProviderService.CNBExchangeRateProviderService.Dto;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderServiceDemo : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IExchangeRateProviderService _exchangeRateProviderService;

        private readonly IEnumerable<CurrencyDto> currencies = new[]
        {
            new CurrencyDto{ Code = "USD" },
            new CurrencyDto{ Code = "EUR" },
            new CurrencyDto{ Code = "CZK" },
            new CurrencyDto{ Code = "JPY" },
            new CurrencyDto{ Code = "KES" },
            new CurrencyDto{ Code = "RUB" },
            new CurrencyDto{ Code = "THB" },
            new CurrencyDto{ Code = "TRY" },
            new CurrencyDto{ Code = "XYZ" },
        };

        public ExchangeRateProviderServiceDemo(ILogger<ExchangeRateProviderServiceDemo> logger, IExchangeRateProviderService exchangeRateProviderService)
        {
            _logger = logger;
            _exchangeRateProviderService = exchangeRateProviderService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                var exchangeRates = await _exchangeRateProviderService.GetExchangeRatesAsync(currencies);

                foreach (var exchangeRate in exchangeRates)
                {
                    Console.WriteLine(
                        $"{exchangeRate.BaseCurrency.Code}/{exchangeRate.TargetCurrency.Code}={exchangeRate.Rate}");
                }

                Console.WriteLine("Press CTRL + C to exit");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }

        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddHostedService<ExchangeRateProviderServiceDemo>();
            builder.Services.AddExchangeRateProviderDependencies();

            // Build and run the demo
            var app = builder.Build();
            app.Run();
        }
    }
}
