namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Models;

/// <summary>
/// Represents the complete daily exchange rate data from CNB.
/// </summary>
/// <param name="Date">The date of the exchange rates.</param>
/// <param name="Sequence">Sequential number of the publication. Represents the number of working day according to Czech bank holidays</param>
/// <param name="ExchangeRates">Collection of exchange rate records.</param>
public record DailyExchangeRatesResponse(
    DateOnly Date,
    int Sequence,
    IReadOnlyList<ExchangeRate> ExchangeRates
);