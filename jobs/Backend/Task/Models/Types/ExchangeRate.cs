namespace ExchangeRateUpdater.Models.Types;
internal record ExchangeRate(Currency SourceCurrency, Currency TargetCurrency, Rate Rate);