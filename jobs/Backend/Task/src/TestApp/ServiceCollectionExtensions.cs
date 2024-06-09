using ExchangeRateProvider.BankApiClients.Cnb;
using ExchangeRateProvider.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using Polly;

namespace TestApp;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddExchangeRateProviderServices(this IServiceCollection services, HostBuilderContext context)
	{
		services
			.AddSingleton(TimeProvider.System)
			.AddLogging(builder => builder.AddConsole())
			.AddMemoryCache(options =>
			{
				options.SizeLimit = 1024;
			})
			.AddSingleton<IExchangeRateProvider, ExchangeRateProvider.ExchangeRateProvider>()
			.AddHttpClient<IBankApiClient, CnbBankApiClient>(client =>
			{
				var baseUrl = context.Configuration["BaseUrl"] ??
				              throw new Exception("BaseUrl configuration not found.");

				client.BaseAddress = new Uri(baseUrl);
			})
			.AddStandardResilienceHandler(options =>
			{
				options.Retry.MaxRetryAttempts = 4;
				options.Retry.BackoffType = DelayBackoffType.Exponential;
				options.Retry.UseJitter = true;
			});
		services.AddOpenTelemetry()
			.WithMetrics(options =>
			{
				options
					.AddMeter("ExchangeRateProvider")
					.AddConsoleExporter();
			});
		return services;
	}

}