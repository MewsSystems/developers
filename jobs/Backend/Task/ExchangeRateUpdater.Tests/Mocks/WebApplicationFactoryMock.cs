using ExchangeRateUpdater.Core.Clients.CNB;
using ExchangeRateUpdater.Host;
using ExchangeRateUpdater.Logic.Clients.CNB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRateUpdater.Tests.Mocks
{
	public class WebApplicationFactoryMock : WebApplicationFactory<Program>
	{
		public HttpMessageHandlerMock MessageHandlerMock { get; set; } = new HttpMessageHandlerMock();

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureTestServices(services =>
			{
				services.AddHttpClient<ICzechNationalBankService, CzechNationalBankService>()
				.ConfigurePrimaryHttpMessageHandler(() => MessageHandlerMock);
			});
		}
	}
}
