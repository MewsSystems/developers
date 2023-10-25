using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.ApiModels;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.Extensions;

internal static class CzechNationalBankExchangeRateExtensions
{
	public static ExchangeRate To(this CzechNationalBankExchangeRate rawExchangeRate) =>
		new(new Currency(rawExchangeRate.CurrencyCode), new Currency("CZK"), rawExchangeRate.Rate);
}