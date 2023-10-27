using System.Collections.Generic;
using ExchangeRateUpdater.Core.Models;

namespace ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi.ApiModels;

internal class CzechNationalBankBaseExchangeRate
{
	public IEnumerable<CzechNationalBankExchangeRate> Rates { get; set; }
}

internal class CzechNationalBankExchangeRate
{
	public string CurrencyCode { get; set; } = null!;
	public decimal Rate { get; set; }
}