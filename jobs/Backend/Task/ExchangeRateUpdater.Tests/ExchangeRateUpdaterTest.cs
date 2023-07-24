using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateUpdaterTest
{
   [Fact]
   public void OneExistingRate()
   {
      var provider = new ExchangeRateProvider(new ExchangeRateService());
      var rates = provider.GetExchangeRates(new Currency[] {
         new Currency("EUR")
      });

      var rate = rates.Single();

      Assert.Equal("EUR", rate.SourceCurrency.Code);
      Assert.Equal("CZK", rate.TargetCurrency.Code);
      Assert.Equal(24, rate.Value);
   }
}