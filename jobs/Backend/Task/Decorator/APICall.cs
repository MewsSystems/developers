using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Helpers;

namespace ExchangeRateUpdater.CNB
{
    internal class APICall : LoadRates
    {
        string path;

        readonly HttpClient client;
        readonly StringBuilder result;
        
        HttpResponseMessage response;

        public APICall(ILoadRates loadRates) : base(loadRates)
        {
            path = ConfigHelper.GetCnbApiPath();
            
            client = new();
            result = new();
        }

        public override async Task<bool> Load(string data)
        {
            if (Regex.IsMatch(data, @"^([0-2]?[0-9]|3[0-1])\.(0[1-9]|1[0-2])\.\d{4}$"))
            {
                path += $"?date = {data}";
            }

            return await load();
        }

        private async Task<bool> load()
        {
            response = await client.GetAsync(path);

            if (response.IsSuccessStatusCode)
            {
                result.Append(await response.Content.ReadAsStringAsync());
            }

            return await wrapper.Load(result.ToString());
        }
    }
}
