using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater.CNB
{
    public class APICall : LoadRates
    {
        string path;

        readonly HttpClient client;
        StringBuilder result;
        
        HttpResponseMessage response;

        public APICall(ILoadRates loadRates) : base(loadRates)
        {
            path = ConfigHelper.GetCnbApiPath();
            client = new();
        }

        public APICall(ILoadRates loadRates, HttpClient httpClient) : base(loadRates)
        {
            path = ConfigHelper.GetCnbApiPath();
            client = httpClient;
        }

        public override async Task<bool> Load(string data)
        {
            try
            {
                if (Regex.IsMatch(data, @"^([0-2]?[0-9]|3[0-1])\.(0[1-9]|1[0-2])\.\d{4}$"))
                {
                    path += $"?date = {data}";
                }
            }
            catch(ArgumentNullException e)
            {
                Console.WriteLine($"Wrong input: '{e.Message}'.");
            }

            return await load();
        }

        private async Task<bool> load()
        {
            result = new();

            try
            {
                response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    result.Append(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    throw new Exception("No data from the resource");
                }
            }
            catch(HttpRequestException)
            {
                throw;
            }

            return await wrapper.Load(result.ToString());
        }
    }
}
