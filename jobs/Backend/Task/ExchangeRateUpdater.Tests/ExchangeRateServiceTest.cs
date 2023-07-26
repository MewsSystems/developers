using System.Linq;
using System.Net.Http;
using System.Text.Json;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateServiceTest
{
   [Fact]
   public async void MapRetrievedCzkExchangeRate()
   {
      var stub = new HttpMessageHandlerStub();

      stub.AddResponse(
         JsonSerializer.Serialize(new ExchangeRateServiceResponse()
         {
            Rates = new ExchangeRateResponse[]
            {
               new ExchangeRateResponse()
               {
                  CurrencyCode = "USD",
                  Rate = 21
               }
            }
         })
      );

      var service = new ExchangeRateService(new HttpClient(stub));
      var response = await service.Get();
      var rate = response.Rates.Single();

      Assert.Equal("USD", rate.CurrencyCode);
      Assert.Equal(21, rate.Rate);
   }
}