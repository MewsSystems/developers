using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.ApiModels;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.Extensions;
using ExchangeRateUpdater.Core.Extensions;
using ExchangeRateUpdater.Core.Models;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;

internal class CzechNationalBankApiAdapter : IExchangeRateApiAdapter
{
	private readonly HttpClient _httpClient;

	public CzechNationalBankApiAdapter(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<IEnumerable<ExchangeRate>> GetExchangesRateAsync(CancellationToken cancellationToken)
	{
		var path = "cnbapi/exrates/daily";

		var result = await _httpClient.GetAsync(path, cancellationToken);

		result.StatusCode.ThrowIfNoSuccessful();
		cancellationToken.ThrowIfCancellationRequested();
		var content =
			JsonConvert.DeserializeObject<CzechNationalBankBaseExchangeRate>(
				await result.Content.ReadAsStringAsync(cancellationToken));

		return content?.Rates.Select(x => x.To()) ?? Enumerable.Empty<ExchangeRate>();
	}
}