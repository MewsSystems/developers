using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;
using Microsoft.Extensions.Options;

namespace ERU.Application.Services.ExchangeRate;

public class CnbExchangeRateMapper: 
	IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>
{
	private readonly ConnectorSettings _connectorSettings;

	public CnbExchangeRateMapper(IOptions<ConnectorSettings> connectorSettingsConfiguration)
	{
		_connectorSettings = connectorSettingsConfiguration.Value;
	}

	TResult IContractObjectMapper<CnbExchangeRateResponse, Domain.ExchangeRate>.Map<TInput, TResult>(TInput inputObject)
	{
		string? mainCurrency = _connectorSettings.SourceCurrency;
		if (inputObject.Amount is null or 0)
		{
			throw new InvalidMapperUse(nameof(inputObject.Amount));
		}
		decimal? outputRate = inputObject.Rate / inputObject.Amount;
		return (TResult)new Domain.ExchangeRate(
			new Currency(mainCurrency ?? throw new InvalidConfigurationException(nameof(_connectorSettings.SourceCurrency))),
			new Currency(inputObject.Code ?? throw new InvalidMapperUse(nameof(inputObject.Code))),
			outputRate ?? throw new InvalidMapperUse(nameof(inputObject.Rate)));
	}
}