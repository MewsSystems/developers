using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Client
{
	public class ExchangeRateClient : IExchangeRateClient
	{
		public async Task<IEnumerable<ExchangeRateEntity>> GetExchangeRateEntitiesAsync(IEnumerable<Currency> currencies)
		{			
			var entities = Enumerable.Empty<ExchangeRateEntity>();
			var client = new HttpClient();

			var response = await client.GetAsync(new Uri(ExchangeRateSettings.CnbExchangeRatesGetPath));

			if (response.IsSuccessStatusCode)
			{
					var dto = await response.Content.ReadFromJsonAsync<ExchangeRatesDto>();
					entities = dto.ExchangeRates;
			}
						
			return entities;
		}
	}
}