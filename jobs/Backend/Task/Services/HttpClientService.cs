using System;
using System.IO;
using System.Net.Http;

namespace ExchangeRateUpdater.Services
{
    public static class HttpClientService
    {
        private const string BaseUrlCzBankFrequentCurrency = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt;jsessionid=70B9F9727E1A51FDF29ECAD25031654B";

        private const string UrlDateFormat = "dd.MM.yyyy";

        private const string BaseUrlCzBankOtherCurrency = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";

        public static string? GetDataFromCzBankFrequentCurrency(DateTime date)
        {
            try
            {
                var url = BaseUrlCzBankFrequentCurrency + "?date=" + date.ToString(UrlDateFormat);
                return GetDataFromRequest(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HttpClientService.GetDataFromCzBankFrequentCurrency\n" + ex.Message);
                return null;
            }
        }

        public static string? GetDataFromCzBankOtherCurrency(DateTime date)
        {
            try
            {
                var url = BaseUrlCzBankOtherCurrency + "?year=" + date.Year + "&month=" + date.Month;
                return GetDataFromRequest(url);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HttpClientService.GetDataFromCzBankOtherCurrency\n" + ex.Message);
                return null;
            }
        }

        private static string? GetDataFromRequest(string url)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = client.Send(request);
            response.EnsureSuccessStatusCode();

            using var responseBody = response.Content.ReadAsStream();
            using var reader = new StreamReader(responseBody);
            string responseString = reader.ReadToEnd();

            return responseString;
        }
    }
}
