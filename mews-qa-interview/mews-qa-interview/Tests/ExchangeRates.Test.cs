using MewsQaInterview.Objects;
using MewsQaInterview.Objects.Response;
using NUnit.Framework;
using System.Linq;

namespace MewsQaInterview.Tests
{
    class ExchangeRatesTest
    {

        [Test]
        public void When_GetExhange_Expect_ValidDataReturned()
        {
            /*
             * Create a Post request on exchangeRates/getAll endpoint.
             * Validate the data returned.
             */
            var requestJson = new RequestObject();
            var response = CreateRequest.Post<GetExhangeRates>("exchange", requestJson);
            var rates = response.ExchangeRates;

            Assert.Multiple(() =>
           {
               Assert.IsNotEmpty(rates,
                   "List of exchange rates should not be empty");
               Assert.IsEmpty(rates.Where(r => r.SourceCurrency.Equals(string.Empty)).ToList(),
                   "Source currency should not be empty");
               Assert.IsEmpty(rates.Where(r => r.TargetCurrency.Equals(string.Empty)).ToList(),
                   "Target currency should not be empty");
               Assert.IsEmpty(rates.Where( r => r.Value < 0).ToList(),
                   "Values should not be negative");
           });
        }
    }
}
