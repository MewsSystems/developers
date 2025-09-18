using ExchangeRateUpdater.Src;
using ExchangeRateUpdater.Src.Cnb;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.UnitTests;

[TestFixture]
public class ExchangeRateProviderTests
{
    [Test]
    public async Task JsonApi_ParsesAndCaches_OnSecondCallHitsCache()
    {
        var json = """
        [
          {"CurrencyCode":"EUR","Amount":1,"Rate":25.123,"ValidFor":"2025-09-12"},
          {"CurrencyCode":"USD","Amount":1,"Rate":22.987,"ValidFor":"2025-09-12"}
        ]
        """;

        int httpHits = 0;
        var handler = new FakeHandler(_ =>
        {
            httpHits++;
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        });

        var http = new HttpClient(handler);
        var services = new ServiceCollection()
            .AddDistributedMemoryCache()
            .BuildServiceProvider();

        var cache = services.GetRequiredService<IDistributedCache>();
        var opts = Options.Create(new CnbOptions());
        var provider = new ExchangeRateProvider(http, cache, opts, NullLogger<ExchangeRateProvider>.Instance);

        var date = new DateOnly(2025, 9, 12);

        var r1 = await provider.GetAsync(date);
        Assert.That(r1.Count, Is.EqualTo(2));
        var eur = r1.First(x => x.SourceCurrency == "EUR");
        Assert.That(eur.Value, Is.EqualTo(25.123m));
        Assert.That(eur.ValidFor, Is.EqualTo(date));

        var r2 = await provider.GetAsync(date);
        Assert.That(r2.Count, Is.EqualTo(2));
        Assert.That(httpHits, Is.EqualTo(1)); // cached
    }

    [Test]
    public async Task JsonApi_StaleIfError_ServesPreviousBusinessDayFromCache()
    {
        var resolved = new DateOnly(2025, 9, 12);
        var prevDay = new DateOnly(2025, 9, 11);

        var seeded = new[]
        {
            new ExchangeRate("EUR", "CZK", 25.000m, prevDay),
            new ExchangeRate("USD", "CZK", 23.000m, prevDay),
        };
        var seedJson = JsonSerializer.Serialize(seeded);

        var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.InternalServerError));
        var http = new HttpClient(handler);

        var services = new ServiceCollection()
            .AddDistributedMemoryCache()
            .BuildServiceProvider();

        var cache = services.GetRequiredService<IDistributedCache>();
        var opts = Options.Create(new CnbOptions());

        var keyPrev = $"{opts.Value.CacheKeyPrefix}{prevDay:yyyy-MM-dd}";
        await cache.SetStringAsync(keyPrev, seedJson, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        });

        var provider = new ExchangeRateProvider(http, cache, opts, NullLogger<ExchangeRateProvider>.Instance);
        var result = await provider.GetAsync(resolved);

        Assert.That(result.First().ValidFor, Is.EqualTo(prevDay));
        Assert.That(result.Any(r => r.SourceCurrency == "EUR" && r.Value == 25.000m), Is.True);
    }

    [Test]
    public async Task JsonApi_ParsesEnvelopeWithItems()
    {
        var json = """
        {
          "date": "2025-09-12",
          "items": [
            {"CurrencyCode":"GBP","Amount":1,"Rate":"29.876","ValidFor":"2025-09-12"}
          ]
        }
        """;

        var handler = new FakeHandler(_ => new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        });

        var http = new HttpClient(handler);
        var services = new ServiceCollection()
            .AddDistributedMemoryCache()
            .BuildServiceProvider();

        var cache = services.GetRequiredService<IDistributedCache>();
        var opts = Options.Create(new CnbOptions());
        var provider = new ExchangeRateProvider(http, cache, opts, NullLogger<ExchangeRateProvider>.Instance);

        var date = new DateOnly(2025, 9, 12);
        var rates = await provider.GetAsync(date);

        Assert.That(rates.Count, Is.EqualTo(1));
        var gbp = rates.Single();
        Assert.That(gbp.SourceCurrency, Is.EqualTo("GBP"));
        Assert.That(gbp.Value, Is.EqualTo(29.876m));
        Assert.That(gbp.ValidFor, Is.EqualTo(date));
    }

    private sealed class FakeHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> _responder;
        public FakeHandler(Func<HttpRequestMessage, HttpResponseMessage> responder) => _responder = responder;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(_responder(request));
    }
}
