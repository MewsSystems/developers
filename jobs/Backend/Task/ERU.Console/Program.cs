using ERU.Application;
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
				services.Configure<ConnectorSettings>(hostContext.Configuration.GetSection(nameof(ConnectorSettings)));
				services.Configure<CacheSettings>(hostContext.Configuration.GetSection(nameof(CacheSettings)));
				services.AddOptions();
				services.RegisterApplicationLayerServices();
				
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