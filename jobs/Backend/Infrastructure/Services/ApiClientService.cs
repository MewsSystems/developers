using System;
using System.Net.Http;
using System.Threading.Tasks;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly HttpClient _client;

        public ApiClientService()
        {
            _client = new HttpClient();
        }

        public async Task<string> ConsumeEndpoint(string url)
        {
            try
            {
                return await _client.GetStringAsync(url);
            }

            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }
        }
    }
}
