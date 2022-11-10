using System.Collections.Concurrent;
using System.Globalization;
using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;

namespace ERU.Application.Services.ExchangeRate;

public class CnbDataExtractor : IDataExtractor
{
	private readonly IHttpClient _client;
	private readonly IDataStringParser<IEnumerable<CnbExchangeRateResponse>> _parser;
	private readonly IEnumerable<string> _fileUrls;
	private readonly ICache _memoryCache;

	public CnbDataExtractor(IHttpClient client, 
							IDataStringParser<IEnumerable<CnbExchangeRateResponse>> parser, 
							IEnumerable<string> fileUrls,
							ICache memoryCache)
	{
		_client = client;
		_parser = parser;
		_memoryCache = memoryCache;
		_fileUrls = fileUrls;
	}

	public async Task<IEnumerable<CnbExchangeRateResponse>> ExtractCnbData(IReadOnlyCollection<string> currencyCodes, CancellationToken token)
	{
		if (currencyCodes == null || !currencyCodes.Any())
			throw new ArgumentNullException(nameof(currencyCodes));
		if (_fileUrls == null || !_fileUrls.Any())
			throw new InvalidConfigurationException(nameof(_fileUrls));
		string cacheKey = $"{DateTime.UtcNow.Date.ToString(CultureInfo.InvariantCulture)}-{nameof(CnbDataExtractor)}-{nameof(ExtractCnbData)}";
		var allRates = _memoryCache.GetFromCache<IEnumerable<CnbExchangeRateResponse>>(cacheKey);
		if (allRates != null)
			return allRates;

		allRates = await SearchUntilFindAllCodes(_fileUrls.ToList(), currencyCodes, token);
		_memoryCache.InsertToCache(cacheKey, allRates);
		return allRates;
	}

	/// <summary>
	/// Searches for the desired codes in all the files, if all the required rares are found, the search stops.
	/// </summary>
	private async Task<IEnumerable<CnbExchangeRateResponse>> SearchUntilFindAllCodes(IReadOnlyCollection<string> urls,
		IReadOnlyCollection<string> currencyCodeList, CancellationToken token)
	{
		var tasks = new HashSet<Task<IEnumerable<CnbExchangeRateResponse>>>();
		var results = new ConcurrentBag<CnbExchangeRateResponse>();
		var cts = CancellationTokenSource.CreateLinkedTokenSource(token);
		foreach (string url in urls)
			tasks.Add(GetDataAndParse(url, cts.Token));

		void AddRangeToResults(List<CnbExchangeRateResponse> cnbExchangeRateResponses)
		{
			foreach (CnbExchangeRateResponse rate in cnbExchangeRateResponses)
				results.Add(rate);
		}

		while (tasks.Count > 0)
		{
			var task = await Task.WhenAny(tasks);
			var responses = (await task).ToList();
			if (responses.Select(x => x.Code).Intersect(currencyCodeList).Count() == currencyCodeList.Count())
			{
				cts.Cancel();
				AddRangeToResults(responses);
				return results;
			}

			AddRangeToResults(responses);
			tasks.Remove(task);
		}

		return results;
	}

	private async Task<IEnumerable<CnbExchangeRateResponse>> GetDataAndParse(string url, CancellationToken token)
	{
		string rates = await _client.GetStringAsync(url, token);
		return _parser.Parse(rates);
	}
}