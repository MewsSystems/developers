using ExchangeRate.Console;
using ExchangeRate.Console.Abstraction;
using ExchangeRate.Console.Models.Enums;
using ExchangeRate.Service.Extensions;
using Framework.Logging.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

ILogger? logger = null;
try
{
	var host = CreateHost();
	logger = host.Services.GetRequiredService<ILogger<Program>>();

	return (int)await host.Services.GetRequiredService<IExchangeRateConsoleRunner>().ExecuteGetExchangeRates(args);
}
catch (Exception ex)
{
	LogAppCrash(ex);
	return (int)ExitCode.Error;
}

IHost CreateHost()
{
	return Host.CreateDefaultBuilder()
		.ConfigureServices((context, services) =>
		{
			// add logging
			services.AddFrameworkLogging(context.Configuration);
			// register exchange rate service with all dependencies
			services.AddExchangeRateService(context.Configuration);
			// register console runner
			services.AddSingleton<IExchangeRateConsoleRunner, ExchangeRateConsoleRunner>();
		})
		.Build();
}

void LogAppCrash(Exception ex)
{
	if (logger != null)
	{
		logger.LogCritical(ex, "Something went wrong. Please check logs");
	}
	else
	{
		Console.WriteLine("Something went wrong, logger was not initialized using console");
		Console.WriteLine($"Error message: {ex.Message}");
		Console.WriteLine($"Error stack trace: {ex.StackTrace}");
	}
}
