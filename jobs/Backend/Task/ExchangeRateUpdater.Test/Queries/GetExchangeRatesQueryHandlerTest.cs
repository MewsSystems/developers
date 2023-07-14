using ExchangeRateUpdater.Implementation.Queries;
using ExchangeRateUpdater.Interface.DTOs;
using ExchangeRateUpdater.Interface.Services;
using ExchangeRateUpdater.Test.Helpers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.Queries
{
    [TestClass]
    public class GetExchangeRatesQueryHandlerTest
    {
        private Mock<ILogger<GetExchangeRatesQueryHandler>> _loggerMock;
        private readonly Mock<IExchangeRatesCacheService> _exchangeRatesCacheServiceMock;
        private readonly GetExchangeRatesQuery _queryMock;

        public GetExchangeRatesQueryHandlerTest()
        {
            _loggerMock = new Mock<ILogger<GetExchangeRatesQueryHandler>>();
            _exchangeRatesCacheServiceMock = new Mock<IExchangeRatesCacheService>();
            _queryMock = new GetExchangeRatesQuery { Currencies = TestObjects.Currencies };
        }

        [TestMethod]
        public async Task Handle()
        {
            _exchangeRatesCacheServiceMock.Setup(c => c.GetOrCreateExchangeRatesAsync()).ReturnsAsync(TestObjects.ExchangeRateEntityList);

            var handler = new GetExchangeRatesQueryHandler(_loggerMock.Object, _exchangeRatesCacheServiceMock.Object);

            var result = await handler.Handle(_queryMock, CancellationToken.None);

            _exchangeRatesCacheServiceMock.Verify(x => x.GetOrCreateExchangeRatesAsync(), Times.Once);

            Assert.IsInstanceOfType(result, typeof(IEnumerable<ExchangeRateDto>));
            Assert.AreEqual(TestObjects.ExchangeRateEntityList.Count(), result?.Count());

            var expectedCurrencyCodes = TestObjects.ExchangeRateEntityList.Select(c => c.CurrencyCode).OrderBy(x => x).ToList();
            var actualCurrencyCodes = result?.Select(c => c.TargetCurrency?.Code).OrderBy(x => x).ToList();
            Assert.AreEqual(JsonSerializer.Serialize(expectedCurrencyCodes), JsonSerializer.Serialize(actualCurrencyCodes));
        }
    }
}
