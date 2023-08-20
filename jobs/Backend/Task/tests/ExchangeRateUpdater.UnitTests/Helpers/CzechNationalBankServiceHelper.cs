using Moq;
using RestSharp;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.UnitTests.Helpers
{
    public static class CzechNationalBankServiceHelper
    {
        public const decimal EUR_RATE = 24.03m;
        public const decimal USD_RATE = 22.12m;

        public static IRestClient CreateResponsiveMockedCzechNationalBankService()
        {
            var mockedService = new Mock<IRestClient>();

            var response = new RestResponse();
            response.Content = OK_CNB_RESPONSE;
            response.IsSuccessStatusCode = true;
            response.StatusCode = HttpStatusCode.OK;

            mockedService.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), default))
                .Returns(Task.FromResult(response));

            return mockedService.Object;
        }

        // Required to avoid system culture issues
        // eg. Spanish machine uses ',' as decimal separator
        private static string DecimalToString(decimal d) => d.ToString(new CultureInfo("en-US"));

        private static string OK_CNB_RESPONSE =>
            @"{ 
                'rates':
                [
                    {
			            'validFor': '2023-08-18',
                        'order': 159,
			            'country': 'EMU',
			            'currency': 'euro',
			            'amount': 1,
			            'currencyCode': 'EUR',
			            'rate':" + DecimalToString(EUR_RATE) + @"
                    },{
			            'validFor': '2023-08-18',
                        'order': 159,
			            'country': 'USA',
			            'currency': 'dollar',
			            'amount': 1,
			            'currencyCode': 'USD',
			            'rate': " + DecimalToString(USD_RATE) + @"
                    }
                ]
            }";
    }
}
