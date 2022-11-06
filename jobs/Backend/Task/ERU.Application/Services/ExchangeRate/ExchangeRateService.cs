using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;
using Microsoft.Extensions.Options;

namespace ERU.Application.Services.ExchangeRate;

public class ExchangeRateService : IExchangeRateProvider
{
	private readonly IHttpClient _client;
	private readonly IDataStringParser<IEnumerable<CnbExchangeRateResult>> _parser;
	private readonly ConnectorSettings _connectorSettings;
	private readonly IContractObjectMapper<CnbExchangeRateResult, Domain.ExchangeRate> _responseMapper;
	
	

	public ExchangeRateService(IHttpClient client, IDataStringParser<IEnumerable<CnbExchangeRateResult>> parser, IOptions<ConnectorSettings> connectorSettingsConfiguration, 
		IContractObjectMapper<CnbExchangeRateResult, Domain.ExchangeRate> responseMapper)
	{
		_client = client;
		_parser = parser;
		_connectorSettings = connectorSettingsConfiguration.Value;
		_responseMapper = responseMapper;
	}

	public async Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken token)
	{
		// TODO: get only currencies that are not in cache
		// TODO: stop searching by additional urls when all currencies are found
		IEnumerable<CnbExchangeRateResult>[] response = await Task.WhenAll(_connectorSettings.FileUri.Select(a=> NewMethod(a,token)));
		IEnumerable<CnbExchangeRateResult> allRates = response.SelectMany(a => a);
		IEnumerable<CnbExchangeRateResult> selectedRates = allRates.Where(a => currencies.Select(cur=>cur.Code).Contains(a.Code)) 
		                                                   ?? throw new EmptyExchangeRateResponseException("No exchange rates were found.");
		IEnumerable<Domain.ExchangeRate> validatedResponse = selectedRates.
			Select(a => Contract.Check(a, _responseMapper, "CNB ExchangeRates API response contract failure."));
		return validatedResponse;
	}

	private async Task<IEnumerable<CnbExchangeRateResult>> NewMethod(string url, CancellationToken token)
	{
		string rates = await _client.GetStringAsync(url, token);
		// TODO: dictionary of currencies and their codes? 
		return _parser.Parse(rates);
	}
	
	// async Task<IEnumerable<Domain.ExchangeRate>> SearchAllByANI(string ani)
	// {
	// 	var tasks = new HashSet<Task<string>>();
	// 	var cts = new CancellationTokenSource();
	// 	var urls = new List<string>{"url1", "url2", "url3"};
	// 	foreach (string url in urls)
	// 	{
	// 		tasks.Add( _client.GetStringAsync(url, cts.Token));
	// 	}
	// 	
	// 	while (tasks.Count > 0)
	// 	{
	// 		var task = await Task.WhenAny(tasks);
	// 		var result = await task;
	// 		if (result != null && result.Any())
	// 		{
	// 			cts.Cancel();
	// 			return result;
	// 		}
	// 		tasks.Remove(task);
	// 	}
	// 	return null;
	// }
	
}