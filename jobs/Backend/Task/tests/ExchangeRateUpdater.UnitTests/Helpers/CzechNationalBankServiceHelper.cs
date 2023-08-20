using Moq;
using RestSharp;
using System.Collections.Generic;
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

            mockedService.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), default))
                .Returns(Task.FromResult(GetSuccessResponse()));

            return mockedService.Object;
        }

        public static IRestClient CreateErroringMockedCzechNationalBankService(HttpStatusCode httpStatusCode)
        {
            var mockedService = new Mock<IRestClient>();

            mockedService.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), default))
                .Returns(Task.FromResult(GetErrorResponse(httpStatusCode)));

            return mockedService.Object;
        }

        public static Mock<IRestClient> CreateTransientErrorMockedCzechNationalBankService()
        {
            var mockedService = new Mock<IRestClient>();

            var responses = new Queue<RestResponse>();
            responses.Enqueue(GetErrorResponse(HttpStatusCode.InternalServerError));
            responses.Enqueue(GetErrorResponse(HttpStatusCode.InternalServerError));
            responses.Enqueue(GetErrorResponse(HttpStatusCode.InternalServerError));
            responses.Enqueue(GetSuccessResponse());

            mockedService.Setup(c => c.ExecuteAsync(It.IsAny<RestRequest>(), default))
                .Returns(() => Task.FromResult(responses.Dequeue()));

            return mockedService;
        }

        private static RestResponse GetSuccessResponse()
        {
            var response = new RestResponse();

            response.Content = OK_CNB_RESPONSE;
            response.IsSuccessStatusCode = true;
            response.StatusCode = HttpStatusCode.OK;

            return response;
        }

        private static RestResponse GetErrorResponse(HttpStatusCode httpStatusCode)
        {
            var response = new RestResponse();

            response.Content = ERROR_CNB_RESPONSE;
            response.IsSuccessStatusCode = false;
            response.StatusCode = httpStatusCode;

            return response;
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

        private static string ERROR_CNB_RESPONSE =>
            @"{
                  'description': 'Something went wrong',
                  'endPoint': 'endpoint',
                  'errorCode': 'CNB_ERROR_CODE',
                  'happenedAt': '2023-08-20T06:57:29.098Z',
                  'messageId': 'cnb-id'
            }";
    }
}
