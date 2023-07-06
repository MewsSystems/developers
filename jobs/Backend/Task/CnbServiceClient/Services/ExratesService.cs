using CnbServiceClient.Interfaces;
using CnbServiceClient.DTOs;

namespace CnbServiceClient.Services
{
	public class ExratesService : BaseService, IExratesService
	{
        public ExratesService(HttpClient httpClient)
            : base(httpClient)
        { }

        public async Task<IEnumerable<Exrate>> GetExratesDailyAsync()
        {
            var response = await GetAsync<Exrates>("cnbapi/exrates/daily");

            return response.Rates;
        }
    }
}

