using System.Globalization;
using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;

namespace ERU.Application.Services.ExchangeRate;

public class ExchangeRateService : IExchangeRateProvider
{
	private readonly IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate> _responseMapper;
	private readonly ICnbDataExtractor _cnbDataExtractor;

	public ExchangeRateService(IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate> responseMapper, ICnbDataExtractor cnbDataExtractor)
	{
		_responseMapper = responseMapper;
		_cnbDataExtractor = cnbDataExtractor;
	}

	public async Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken token)
	{
		if (currencies == null || !currencies.Any()) 
		{
			throw new ArgumentNullException(nameof(currencies));
		}
		string cacheKey = $"{DateTime.UtcNow.Date.ToString(CultureInfo.InvariantCulture)}-{nameof(ExchangeRateService)}-{nameof(GetExchangeRates)}";
		var allRates = await _cnbDataExtractor.CnbExchangeRateResults(currencies.Select(cur=>cur.Code), cacheKey, token);
		var selectedRates = allRates.Where(a => currencies.Select(cur=>cur.Code).Contains(a.Code)) 
		                    ?? throw new EmptyExchangeRateResponseException("No exchange rates were found.");
		return selectedRates.
			Select(a => Contract.Check(a, _responseMapper, "CNB ExchangeRates API response contract failure."));
	}
}