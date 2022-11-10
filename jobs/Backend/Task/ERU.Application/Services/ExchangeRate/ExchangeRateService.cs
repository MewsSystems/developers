using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;

namespace ERU.Application.Services.ExchangeRate;

public class ExchangeRateService : IExchangeRateProvider
{
	private readonly IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate> _responseMapper;
	private readonly IDataExtractor _cnbDataExtractor;

	public ExchangeRateService(IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate> responseMapper, IDataExtractor cnbDataExtractor)
	{
		_responseMapper = responseMapper;
		_cnbDataExtractor = cnbDataExtractor;
	}

	public async Task<IEnumerable<Domain.ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken token)
	{
		var currencyList = currencies.ToList();
		if (currencies == null || !currencyList.Any())
			throw new ArgumentNullException(nameof(currencies));
		var currencyCodes = currencyList.Select(cur => cur.Code).ToList();
		var allRates = await _cnbDataExtractor.ExtractCnbData(currencyCodes, token);
		var selectedRates = allRates.Where(a => a.Code != null && currencyCodes.Contains(a.Code))
		                    ?? throw new EmptyExchangeRateResponseException(string.Join(",", currencyCodes));
		return selectedRates.Select(a => Contract.Check(a, _responseMapper, "CNB ExchangeRates API response contract failure."));
	}
}