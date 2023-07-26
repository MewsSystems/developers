using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTest
{
   [Fact]
   public async void OneRate()
   {
      var stub = new ExchangeRateServiceStub();
      stub.Add("EUR", 24);

      var provider = new ExchangeRateProvider(stub);

      var rates = await provider.GetExchangeRates(new Currency[] {
         new Currency("EUR"),
         new Currency("CZK")
      });

      var rate = rates.Single();

      Assert.Equal("EUR", rate.SourceCurrency.Code);
      Assert.Equal("CZK", rate.TargetCurrency.Code);
      Assert.Equal(24, rate.Value);
   }

   [Fact]
   public async void MultipleRates()
   {
      var stub = new ExchangeRateServiceStub();
      stub.Add("EUR", 24);
      stub.Add("USD", 20);

      var provider = new ExchangeRateProvider(stub);

      var rates = await provider.GetExchangeRates(new Currency[] {
         new Currency("EUR"),
         new Currency("CZK"),
         new Currency("USD")
      });

      Assert.Equal(2, rates.Count());

      Assert.Contains(rates, rate =>
         rate.SourceCurrency.Code == "EUR" &&
         rate.TargetCurrency.Code == "CZK" &&
         rate.Value == 24);

      Assert.Contains(rates, rate =>
         rate.SourceCurrency.Code == "USD" &&
         rate.TargetCurrency.Code == "CZK" &&
         rate.Value == 20);
   }
}