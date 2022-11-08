using System.Globalization;
using ERU.Application.DTOs;
using ERU.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace ERU.Application.Services.ExchangeRate;

public class CnbExchangeRateDataParser : IDataStringParser<IEnumerable<CnbExchangeRateResponse>>
{
	private readonly ConnectorSettings _connectorSettings;

	public CnbExchangeRateDataParser(IOptions<ConnectorSettings> connectorSettingsConfiguration)
	{
		_connectorSettings = connectorSettingsConfiguration.Value;
	}

	public IEnumerable<CnbExchangeRateResponse> Parse(string input)
	{
		string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);
		return lines
			.Skip(_connectorSettings.DataSkipLines)
			.Select(ParseExchangeRateLine)
			.Where(line => line != null)!;
	}

	private CnbExchangeRateResponse? ParseExchangeRateLine(string line)
	{
		string[] cells = line.Split('|');
		string rawSourceAmount = cells[_connectorSettings.AmountIndex];
		string rawCurrencyCode = cells[_connectorSettings.CodeIndex];
		string rawRate = cells[_connectorSettings.RateIndex];

		if (decimal.TryParse(rawSourceAmount, out var sourceAmount) 
		    && decimal.TryParse(rawRate, NumberStyles.Currency, CultureInfo.InvariantCulture, out var targetAmount))
		{
			return new CnbExchangeRateResponse(sourceAmount, rawCurrencyCode, targetAmount);
		}

		return null;
	}
}