using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public static class ApiHelper
    {
        private static readonly HttpClient _client;

        static ApiHelper()
        {
            _client = new HttpClient();
        }


        public static async Task<string> ConsumeEndpoint(string url)
        {
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/xml"));
            string response = null;
            try
            {
                response = await _client.GetStringAsync(url);


            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return response;
        }
    }
}
