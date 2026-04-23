using ExchangeRateUpdater.Core.Http;
using ExchangeRateUpdater.Core;
using ExchangeRateUpdater.Infrastructure.Providers;
using System.Text;
using NSubstitute;
using FluentAssertions;

namespace ExchangeRateUpdater.Tests
{
    public class CnbProviderTests
    {
        [Fact]
        public async Task GetExchangeRates_ShouldReturnOnlyRequestedCurrencies()
        {
            #region Arrange

            var response =
                "09 Apr 2025 #70\n" +
                "Country|Currency|Amount|Code|Rate\n" +
                "USA|dollar|1|USD|22.345\n" +
                "Eurozone|euro|1|EUR|24.123\n" +
                "UK|pound|1|GBP|28.100\n";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(response));

            var mockHttpClient = Substitute.For<IHttpClient>();
            mockHttpClient
                .GetStreamAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult<Stream>(stream));

            var provider = new CnbExchangeRateProvider(mockHttpClient);

            var requestedCurrencies = new[]
            {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("JPY")
            };

            #endregion
            #region Act

            var rates = (await provider.GetExchangeRates(requestedCurrencies)).ToList();

            #endregion
            #region Assert

            rates.Should().HaveCount(2);
            rates.Should().ContainSingle(r => r.TargetCurrency.Code == "USD");
            rates.Should().ContainSingle(r => r.TargetCurrency.Code == "EUR");
            rates.Should().NotContain(r => r.TargetCurrency.Code == "JPY");

            #endregion
        }


    }
}