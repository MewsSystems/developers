using ExchangeRateUpdater.Implementations;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using System.IO;

namespace ExchangeRateUpdater
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public Startup(string[] args)
        {
            this.Configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
                   .Build();

            Host.CreateDefaultBuilder(args).ConfigureServices((cntx, services) =>
            {
                services.AddSingleton<IHostEnvironment, HostingEnvironment>();
                services.Configure<BankConfig>(this.Configuration);

                services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                services.AddScoped<IScrapper, Scrapper>();

                services.AddHostedService<ExchangeHostedService>();

            }).RunConsoleAsync().Wait();
        }


    }
}
