using ExchangeRateUpdater.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterTests.WebApi.Fixtures
{
    public class ExchangeRateUpdaterWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureHostConfiguration(config =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string> {
                    { "ExchangeRateProviderUrl", "http://fake-api-url" },
                    { "SourceCurrency", "SCR" }
                });
            });

            builder.ConfigureServices(services =>
            {
                services.AddSingleton<FakeEchangeRateApiHandler>();

                services.AddHttpClient(
                    "ExchangeRatesApiClient",
                    u => u.BaseAddress = new Uri("http://fake-exchange-rate-api"))
                .AddHttpMessageHandler<FakeEchangeRateApiHandler>();
            });

            return base.CreateHost(builder);
        }
    }

    class FakeEchangeRateApiHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
           HttpRequestMessage request,
           CancellationToken cancellationToken)
        {
            var rawContent = File.ReadAllText(@"Files\ExchangeRates.txt");

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
           {
               Content = new StringContent(rawContent)
           });               
        }
    }
}
