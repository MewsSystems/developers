using System;

namespace ExchangeRateUpdater.Models.CzechNationalBankResponse;

public record RecordedRate(int Amount, string Country, string Currency, string CurrencyCode, int Order, decimal Rate, DateTime ValidFor);