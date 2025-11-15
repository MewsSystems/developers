using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.WebAPI.Controllers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Tests
{
    internal class FakeExchangeRateService : IExchangeRateService
    {
        private readonly Dictionary<string, decimal> _definedRates; // maps source currency code -> CZK value
        public FakeExchangeRateService(Dictionary<string, decimal> definedRates)
        {
            _definedRates = definedRates.ToDictionary(k => k.Key.ToUpperInvariant(), v => v.Value);
        }
        public Task<IEnumerable<ExchangeRate>> GetRatesAsync(IEnumerable<Currency> requestedCurrencies)
        {
            var list = new List<ExchangeRate>();
            foreach (var c in requestedCurrencies)
            {
                if (_definedRates.TryGetValue(c.Code.ToUpperInvariant(), out var value))
                {
                    // Only return explicit source->CZK; never generate inverted CZK->source.
                    list.Add(new ExchangeRate(new Currency(c.Code.ToUpperInvariant()), new Currency("CZK"), value));
                }
            }
            return Task.FromResult<IEnumerable<ExchangeRate>>(list);
        }
    }

    public class ExchangeRatesControllerTests
    {
        private ExchangeRatesController CreateController(Dictionary<string, decimal> definedRates)
        {
            var service = new FakeExchangeRateService(definedRates);
            var logger = NullLogger<ExchangeRatesController>.Instance;
            return new ExchangeRatesController(service, logger);
        }

        [Fact]
        public async Task MissingCurrenciesParamReturnsBadRequest()
        {
            var controller = CreateController(new());
            var result = await controller.Get(null!);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task EmptyCurrenciesParamReturnsBadRequest()
        {
            var controller = CreateController(new());
            var result = await controller.Get("   ");
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task AllInvalidCodesReturnsBadRequest()
        {
            var controller = CreateController(new());
            var result = await controller.Get("US,EURO,12,TOOLONG,AA"); // none length==3
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task ValidAndInvalidCodesFiltersInvalid()
        {
            var controller = CreateController(new() { { "USD", 23.5m }, { "EUR", 25m } });
            var result = await controller.Get("USD,EURO,EUR,  ,usd");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            // Should contain USD and EUR only once each
            var entries = payload.Select(x => x.GetType().GetProperty("source")!.GetValue(x)!.ToString()).OrderBy(s => s).ToList();
            Assert.Equal(new[] { "EUR", "USD" }, entries);
        }

        [Fact]
        public async Task DuplicatesAreRemoved()
        {
            var controller = CreateController(new() { { "USD", 23.5m } });
            var result = await controller.Get("USD,usd,Usd");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            Assert.Single(payload);
        }

        [Fact]
        public async Task CaseInsensitiveHandling()
        {
            var controller = CreateController(new() { { "USD", 23.5m } });
            var result = await controller.Get("usd");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            var entry = Assert.Single(payload);
            var source = entry.GetType().GetProperty("source")!.GetValue(entry)!.ToString();
            Assert.Equal("USD", source);
        }

        [Fact]
        public async Task UnsupportedCurrenciesIgnored()
        {
            var controller = CreateController(new() { { "USD", 20m } });
            var result = await controller.Get("USD,ABC");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            var entry = Assert.Single(payload);
            var source = entry.GetType().GetProperty("source")!.GetValue(entry)!.ToString();
            Assert.Equal("USD", source);
        }

        [Fact]
        public async Task DoesNotReturnInvertedRates()
        {
            var controller = CreateController(new() { { "USD", 23.5m } });
            var result = await controller.Get("USD,CZK"); // CZK requested but service only defines USD->CZK
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            var entry = Assert.Single(payload); // ensures no CZK/USD inversion added
            var source = entry.GetType().GetProperty("source")!.GetValue(entry)!.ToString();
            var target = entry.GetType().GetProperty("target")!.GetValue(entry)!.ToString();
            Assert.Equal("USD", source);
            Assert.Equal("CZK", target);
        }

        [Fact]
        public async Task EmptyResultWhenNoDefinedRatesMatch()
        {
            var controller = CreateController(new());
            var result = await controller.Get("USD,EUR");
            var ok = Assert.IsType<OkObjectResult>(result.Result);
            var payload = Assert.IsAssignableFrom<IEnumerable<object>>(ok.Value);
            Assert.Empty(payload);
        }
    }
}
