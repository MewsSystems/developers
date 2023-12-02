using Adapter.ExchangeRateProvider.InMemory;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;


namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit
{
    internal abstract class ControllerTestBase
    {
        protected IHost? Host;
        protected TestServer Server;
        protected HttpClient HttpClient;
        protected const string ApiBaseAddress = "http://exchange-rate-update.com";
        protected ExchangeRateProviderRepositoryInMemory ExchangeRateProviderRepository;

        [SetUp]
        public async Task SetUp()
        {
            ExchangeRateProviderRepository = new ExchangeRateProviderRepositoryInMemory();
            var hostBuilder = new TestApplicationHostBuilder(ExchangeRateProviderRepository);
            Host = hostBuilder.Configure().Build();
            await Host.StartAsync();
            Server = Host.GetTestServer();
            Server.BaseAddress = new Uri(ApiBaseAddress);
            HttpClient = Server.CreateClient();
        }

        [TearDown]
        public async Task TearDown()
        {
            HttpClient?.Dispose();
            Server?.Dispose();
            await Host!.StopAsync();
            Host?.Dispose();
        }
    }
}
