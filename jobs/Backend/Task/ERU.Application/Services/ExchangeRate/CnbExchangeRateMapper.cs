using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;

namespace ERU.Application.Services.ExchangeRate;

public class CnbExchangeRateMapper : IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>
{
	private readonly string _sourceCurrencyCode;

	public CnbExchangeRateMapper(string sourceCurrencyCode)
	{
		_sourceCurrencyCode = sourceCurrencyCode;
	}

	TResult IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>.Map<TInput, TResult>(TInput inputObject)
	{
		if (inputObject.Amount is null or 0)
			throw new InvalidMapperUse(nameof(inputObject.Amount));
		decimal? outputRate = inputObject.Rate / inputObject.Amount;
		return (TResult)new Domain.ExchangeRate(
			new Currency(_sourceCurrencyCode ?? throw new InvalidConfigurationException(nameof(_sourceCurrencyCode))),
			new Currency(inputObject.Code ?? throw new InvalidMapperUse(nameof(inputObject.Code))),
			outputRate ?? throw new InvalidMapperUse(nameof(inputObject.Rate)));
	}
}