using Adapter.ExchangeRateProvider.InMemory;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Sinks.InMemory;


namespace ExchangeRateUpdater.Host.WebApi.Tests.Unit.ExchangeRateControllerTests
{
    internal abstract class ControllerTestBase
    {
        protected IHost? Host;
        protected TestServer? Server;
        protected HttpClient? HttpClient;
        protected const string ApiBaseAddress = "http://exchange-rate-update.com";
        protected ExchangeRateProviderRepositoryInMemory? ExchangeRateProviderRepository;
        protected Logger? Logger;

        [SetUp]
        public async Task SetUp()
        {
            Logger = new LoggerConfiguration().WriteTo.InMemory().CreateLogger();
            ExchangeRateProviderRepository = new ExchangeRateProviderRepositoryInMemory();
            var hostBuilder = new TestApplicationHostBuilder(ExchangeRateProviderRepository,
                                                             new Configuration.Settings(),
                                                             Logger);
            Host = hostBuilder.Configure().Build();
            await Host.StartAsync();
            Server = Host.GetTestServer();
            Server.BaseAddress = new Uri(ApiBaseAddress);
            HttpClient = Server.CreateClient();
        }

        [TearDown]
        public async Task TearDown()
        {
            Logger?.Dispose();
            HttpClient?.Dispose();
            Server?.Dispose();
            await Host!.StopAsync();
            Host?.Dispose();
        }
    }
}
