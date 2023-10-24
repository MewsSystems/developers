using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string UnexpectedDataFormat = "Unexpected data format from the exchange rate API.";
        public const string UnexpectedLineFormat = "Unexpected line format";
        public const string ErrorParsingDataInLine = "Error parsing data in line";
        public const string HttpClientInitialization = "HttpClient instance is required.";
        public const string InvalidCurrencyCode = "Invalid currency code provided.";
        public const string InvalidFetchResponse = "Failed to fetch or parse exchange rates.";


        public const string FailedToFetchDataFromAPI = "Failed to fetch data from the exchange rate API.";
        public const string WrongResponseStatusCode = "API returned status code";
    }
}
