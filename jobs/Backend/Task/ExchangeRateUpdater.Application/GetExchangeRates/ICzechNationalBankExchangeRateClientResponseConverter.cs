using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Application.GetExchangeRates;

public interface ICzechNationalBankExchangeRateClientResponseConverter
{
    List<ExchangeRate> Convert(string? clientResponse);
}