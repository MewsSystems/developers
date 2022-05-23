using System.Net;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Client.Cnb.Models;
using Framework.Converters.Extensions;
using Framework.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;

namespace ExchangeRate.Client.Cnb.Extensions
{
	public static class ConfigureServicesExtensions
	{
		public static void AddExchangeRateClientCnbServices(this IServiceCollection services, IConfiguration configuration)
		{
			//register Cnb configuration
			RegisterCnbConfiguration(services, configuration);

			// get retry value used for retry strategy for CnbExchangeRateClient
			var retry = GetRetryValue(services, configuration);

			// create Exchange rate client with retry strategy
			services.AddHttpClient<IExchangeRateClient, CnbExchangeRateClient>()
				.SetHandlerLifetime(TimeSpan.FromMinutes(1))
				.AddPolicyHandler(GetRetryPolicy(retry)
				);

			services.AddFrameworkConverters();

			services.AddScoped<IExchangeRateClient, CnbExchangeRateClient>();
		}

		#region private members

		private static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retry)
		{
			return HttpPolicyExtensions
				.HandleTransientHttpError()
				.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
				.WaitAndRetryAsync(retry, retryAttempt => TimeSpan.FromSeconds(Math.Pow(10, retryAttempt)));
		}

		private static void RegisterCnbConfiguration(IServiceCollection services, IConfiguration configuration)
		{
			try
			{
				services.Configure<CnbClientConfiguration>(configuration.GetSection("CnbClientConfiguration"));
			}
			catch (Exception e)
			{
				const string errorMessage = "Missing CnbClientConfiguration section";
				LogError(services, errorMessage, e);
				throw new ConfigurationException(errorMessage);
			}
		}

		private static int GetRetryValue(IServiceCollection services, IConfiguration configuration)
		{
			var retryString = configuration["CnbClientConfiguration:Retry"];
			if (retryString is not null)
			{
				return Convert.ToInt32(retryString);
			}
			else
			{
				const string errorMessage = "Missing CnbClientConfiguration => Retry value";
				LogError(services, errorMessage);
				throw new ConfigurationException(errorMessage);
			}
		}

		private static void LogError(IServiceCollection services, string message, Exception? e = null)
		{
			ILogger logger = services.BuildServiceProvider().GetRequiredService<ILogger<CnbExchangeRateClient>>();
			logger.LogCritical(e, message);
		}

		#endregion
	}
}
