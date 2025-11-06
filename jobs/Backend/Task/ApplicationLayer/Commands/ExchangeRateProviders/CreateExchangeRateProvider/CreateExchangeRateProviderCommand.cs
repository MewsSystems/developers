using ApplicationLayer.Common.Abstractions;
using DomainLayer.Common;

namespace ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;

/// <summary>
/// Command to create a new exchange rate provider.
/// </summary>
public record CreateExchangeRateProviderCommand(
    string Name,
    string Code,
    string Url,
    int BaseCurrencyId,
    bool RequiresAuthentication,
    string? ApiKeyVaultReference) : ICommand<Result<int>>;
