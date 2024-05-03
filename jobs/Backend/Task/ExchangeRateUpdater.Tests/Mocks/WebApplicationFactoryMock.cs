using ExchangeRateUpdater.Domain.Core.Clients;
using ExchangeRateUpdater.Host;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ExchangeRateUpdater.Tests.Mocks
{
	public class WebApplicationFactoryMock : WebApplicationFactory<Program>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureTestServices(services =>
			{
				services.AddHttpClient<IHttpBankClientWrapper, HttpBankClientWrapperMock>()
				.ConfigureHttpClient(httpClient =>
				{
					httpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("BANK_CLIENT_URL"));
				});
			});
		}
	}
}
