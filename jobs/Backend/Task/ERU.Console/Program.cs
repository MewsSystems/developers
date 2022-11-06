using ERU.Application;
using ERU.Application.Interfaces;
using ERU.Application.Services.ExchangeRate;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ERU.Console;

public static class Program
{
	private static async Task Main(string[] args)
	{
		var configuration = SetupConfiguration();
		await Host.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration((config) =>
			{
				config.AddConfiguration(configuration);
			})
			.ConfigureServices((hostContext, services) =>
			{
				services.AddLogging();
				services.AddHostedService<CurrencyExchangeApp>();
				services.Configure<ConnectorSettings>(hostContext.Configuration.GetSection("ConnectorSettings"));
				services.AddOptions();
				services.RegisterApplicationLayerServices();
				services.AddScoped<IExchangeRateProvider, ExchangeRateService>();
			})
			
			.RunConsoleAsync();
	}

	private static IConfiguration SetupConfiguration()
	{
		return new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", false)
			.Build();
	}
}