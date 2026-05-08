using System.Diagnostics.CodeAnalysis;

namespace ExchangeRateProvider.Api.Constants;

[ExcludeFromCodeCoverage(Justification = "Constants class with only compile-time string values - no executable logic")]
public static class ApiEndpoints
{
    public const string ApiVersion = "v1";
    private const string ApiBase = $"{ApiVersion}/api";

    public static class ExchangeRates
    {
        public const string Base = $"{ApiBase}/exchange-rates";
        public const string GetByCurrency = Base;
    }
}