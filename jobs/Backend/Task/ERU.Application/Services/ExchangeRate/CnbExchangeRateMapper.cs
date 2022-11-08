using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Interfaces;
using ERU.Domain;
using Microsoft.Extensions.Options;

namespace ERU.Application.Services.ExchangeRate;

public class CnbExchangeRateMapper: 
	IContractObjectMapper<CnbExchangeRateResult, Domain.ExchangeRate>
{
	private readonly ConnectorSettings _connectorSettings;

	public CnbExchangeRateMapper(IOptions<ConnectorSettings> connectorSettingsConfiguration)
	{
		_connectorSettings = connectorSettingsConfiguration.Value;
	}

	TResult IContractObjectMapper<CnbExchangeRateResult, Domain.ExchangeRate>.Map<TInput, TResult>(TInput inputObject)
	{
		string? mainCurrency = _connectorSettings.SourceCurrency;
		return (TResult)new Domain.ExchangeRate(
			new Currency(mainCurrency ?? throw new InvalidConfigurationException(nameof(_connectorSettings.SourceCurrency))),
			new Currency(inputObject.Code ?? throw new InvalidMapperUse(nameof(inputObject.Code))),
			inputObject.Rate / inputObject.Amount ?? throw new InvalidMapperUse(nameof(inputObject.Rate))); // TODO: Check if this is correct
	}
}