using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateUpdaterTest
{
   [Fact]
   public void OneRate()
   {
      var stub = new ExchangeRateServiceStub();
      stub.Add("EUR", "CZK", 24);

      var rates =
         new ExchangeRateProvider(stub)
            .GetExchangeRates(new Currency[] {
               new Currency("EUR"),
               new Currency("CZK")
            });

      var rate = rates.Single();

      Assert.Equal("EUR", rate.SourceCurrency.Code);
      Assert.Equal("CZK", rate.TargetCurrency.Code);
      Assert.Equal(24, rate.Value);
   }

   [Fact]
   public void MultipleRates()
   {
      var stub = new ExchangeRateServiceStub();
      stub.Add("EUR", "CZK", 24);
      stub.Add("USD", "CZK", 20);
      stub.Add("USD", "DKK", 7);

      var rates =
         new ExchangeRateProvider(stub)
            .GetExchangeRates(new Currency[] {
               new Currency("EUR"),
               new Currency("CZK"),
               new Currency("USD")
            });

      Assert.Equal(2, rates.Count());

      Assert.True(rates.Any(rate =>
         rate.SourceCurrency.Code == "EUR" &&
         rate.TargetCurrency.Code == "CZK" &&
         rate.Value == 24));

      Assert.True(rates.Any(rate =>
         rate.SourceCurrency.Code == "USD" &&
         rate.TargetCurrency.Code == "CZK" &&
         rate.Value == 20));
   }
}