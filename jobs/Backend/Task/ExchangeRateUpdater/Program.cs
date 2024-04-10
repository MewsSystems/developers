using ExchangeRates.DependencyInjection;
using ExchangeRateUpdater.Presentation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                // Composition Root.
                var hostBuilder = Host.CreateDefaultBuilder()
                    .AddConfiguration()
                    .AddServices()
                    .ConfigureServices((hostContext, services) =>
                    {
                        // Resolution Root.
                        services.AddHostedService(services => new ConsoleService(args, services.GetService<IParser>()));
                    });

                await hostBuilder.RunConsoleAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error executing operation: '{e.Message}'.");
            }

            Console.ReadLine();
        }

        internal class ConsoleService : IHostedService
        {
            private readonly string[] _args;

            private readonly IParser _parser;

            public ConsoleService(string[] args, IParser parser)
            {
                _args = args;
                _parser = parser;
            }

            public async Task StartAsync(CancellationToken cancellationToken)
            {
                var operation = _parser.Parse(_args.AsEnumerable());

                await operation.ExecuteAsync(_args.Skip(1), cancellationToken);
            }

            public Task StopAsync(CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
