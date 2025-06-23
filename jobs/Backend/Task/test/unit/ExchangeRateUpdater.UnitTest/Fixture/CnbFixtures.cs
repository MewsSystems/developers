using System.Net;

namespace ExchangeRateUpdater.UnitTest.Fixture
{
    public class CnbFixtures
    {
        public CnbFixtures()
        {
            HttpCnbOkResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{
  ""rates"": [
    {
      ""validFor"": ""2025-01-13"",
      ""order"": 8,
      ""country"": ""Australia"",
      ""currency"": ""dollar"",
      ""amount"": 1,
      ""currencyCode"": ""AUD"",
      ""rate"": 15.211
    },
    {
      ""validFor"": ""2025-01-13"",
      ""order"": 8,
      ""country"": ""Brazil"",
      ""currency"": ""real"",
      ""amount"": 1,
      ""currencyCode"": ""BRL"",
      ""rate"": 4.058
    }
  ]
}"),
            };
        }

        public HttpResponseMessage HttpCnbOkResponse { get; set; }
    }
}
