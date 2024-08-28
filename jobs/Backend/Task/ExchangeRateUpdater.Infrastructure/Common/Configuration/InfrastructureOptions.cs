namespace ExchangeRateUpdater.Infrastructure.Common.Configuration;

public class InfrastructureOptions
{
    public const string SectionName = "Infrastructure";
    public CzechNationalBankExchangeRateServiceOptions CzechNationalBankExchangeRateService { get; init; }
}