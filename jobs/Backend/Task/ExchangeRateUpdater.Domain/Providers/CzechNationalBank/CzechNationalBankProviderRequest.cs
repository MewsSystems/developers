namespace ExchangeRate.Domain.Providers.CzechNationalBank;

public sealed record CzechNationalBankProviderRequest(string Date, string Lang)
    : IExchangeRateProviderRequest;