using ExchangeRateProviders.Core.Model;
using ExchangeRateProviders.Czk.Config;
using ExchangeRateProviders.Czk.Model;

namespace ExchangeRateProviders.Czk.Mappers;

public static class ContractMapping
{
	public static ExchangeRate MapToExchangeRate(this CnbApiExchangeRateDto dto)
	{
		if (dto is null)
		{
			throw new ArgumentNullException(nameof(dto));
		}

		if (dto.Amount <= 0)
		{
			throw new ArgumentException($"Amount must be positive for currency {dto.CurrencyCode}", nameof(dto));
		}

		var sourceCurrency = new Currency(dto.CurrencyCode.ToUpperInvariant());
		var targetCurrency = new Currency(Constants.ExchangeRateProviderCurrencyCode);
		var perUnitRate = dto.Rate / dto.Amount;

		return new ExchangeRate(sourceCurrency, targetCurrency, perUnitRate, dto.ValidFor);
	}

	public static IEnumerable<ExchangeRate> MapToExchangeRates(this IEnumerable<CnbApiExchangeRateDto> sourceRates)
	{
		if (sourceRates is null)
		{
			throw new ArgumentNullException(nameof(sourceRates));
		}

		var list = sourceRates is ICollection<CnbApiExchangeRateDto> col
			? new List<ExchangeRate>(col.Count)
			: new List<ExchangeRate>();

		foreach (var dto in sourceRates)
		{
			list.Add(dto.MapToExchangeRate());
		}

		return list;
	}
}
