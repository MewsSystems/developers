using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.ConsoleApp;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = CreateHostBuilder(args).Build();
using var scope = host.Services.CreateScope();

try
{
	await scope.ServiceProvider.GetRequiredService<App>().Run(args);
}
catch (Exception ex)
{
	Console.WriteLine($"Unexpected error occurred. {ex.Message}");
}
finally
{
	Console.ReadLine();
}

static IHostBuilder CreateHostBuilder(string[] args)
{
	return Host.CreateDefaultBuilder(args)
				.ConfigureServices((context, services) =>
				{
					services.AddSingleton<App>();

					services.AddInfrastructure(context.Configuration);
					services.AddApplication();
				});
}