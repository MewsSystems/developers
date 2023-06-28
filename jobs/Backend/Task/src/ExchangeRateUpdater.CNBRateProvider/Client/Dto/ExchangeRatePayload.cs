namespace ExchangeRateUpdater.CNBRateProvider.Client.Dto;

/// <summary>
/// Exchange rate CNBRateProvider project response payload (HTTP 200). Full body.
/// </summary>
/// <remarks>
/// See <see href="https://api.cnb.cz/cnbapi/swagger-ui.html#//exrates"/>.
/// </remarks>
internal class ExchangeRateResponse
{
    public IReadOnlyCollection<ExchangeRateMetadata> Rates { get; init; } = null!;
}


internal class ExchangeRateMetadata
{
    public DateTime ValidFor { get; init; }

    public int Order { get; init; }

    public string Country { get; init; } = null!;

    public string Currency { get; init; } = null!;

    public int Amount { get; init; }

    public string CurrencyCode { get; init; } = null!;

    public decimal Rate { get; init; }
}
