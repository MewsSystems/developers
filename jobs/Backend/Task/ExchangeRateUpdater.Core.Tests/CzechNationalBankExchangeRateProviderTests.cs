using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.ApiVendors;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using FluentAssertions;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Core.Tests;

public class CzechNationalBankExchangeRateProviderTests
{
    [Fact]
    public async Task GetExchangeRates_EmptyInput_ReturnsEmpty_AndDoesNotCallVendor()
    {
        // Arrange
        var vendorMock = new Mock<IExchangeRateVendor>(MockBehavior.Strict);
        var sut = new CzechNationalBankExchangeRateProvider(vendorMock.Object);

        // Act
        var result = await sut.GetExchangeRates([]);

        // Assert
        result.Should().BeEmpty();
        vendorMock.Verify(v => v.GetExchangeRates(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task GetExchangeRates_FiltersToRequestedCurrencies_AndCallsVendorOnce()
    {
        // Arrange
        var vendorMock = new Mock<IExchangeRateVendor>();
        vendorMock
            .Setup(v => v.GetExchangeRates("CZK"))
            .ReturnsAsync(new List<ExchangeRate>
            {
                new(new Currency("CZK"), new Currency("USD"), 0.043m),
                new(new Currency("CZK"), new Currency("EUR"), 0.039m),
                new(new Currency("CZK"), new Currency("GBP"), 0.034m)
            });

        var sut = new CzechNationalBankExchangeRateProvider(vendorMock.Object);

        var requested = new[] { new Currency("USD"), new Currency("EUR") };

        // Act
        var result = await sut.GetExchangeRates(requested);

        // Assert
        result.Should().HaveCount(2);
        result.Select(r => r.TargetCurrency.ToString()).Should().BeEquivalentTo(new[] { "USD", "EUR" });
        result.All(r => r.SourceCurrency.ToString() == "CZK").Should().BeTrue();
        vendorMock.Verify(v => v.GetExchangeRates("CZK"), Times.Once);
    }
}
