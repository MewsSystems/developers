using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.ApiModels;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.Extensions;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;

internal class CzechNationalBankApiAdapter : IExchangeRateApiAdapter
{
	private readonly HttpClient _httpClient;
	private readonly IMemoryCache _cache;

	public CzechNationalBankApiAdapter(HttpClient httpClient, IMemoryCache cache)
	{
		_httpClient = httpClient;
		_cache = cache;
	}

	public async Task<IEnumerable<ExchangeRate>> GetExchangesRateAsync(CancellationToken cancellationToken)
	{
		var path = "cnbapi/exrates/daily";

		if (!_cache.TryGetValue(path, out IEnumerable<ExchangeRate> result))
		{
			var respone = await _httpClient.GetAsync(path, cancellationToken);

			respone.StatusCode.ThrowIfNoSuccessful();
		
			var content =
				JsonConvert.DeserializeObject<CzechNationalBankBaseExchangeRate>(
					await respone.Content.ReadAsStringAsync(cancellationToken));

			result = content?.Rates.Select(x => x.To()) ?? Enumerable.Empty<ExchangeRate>();
		}
		
		

		return result!;
	}
}