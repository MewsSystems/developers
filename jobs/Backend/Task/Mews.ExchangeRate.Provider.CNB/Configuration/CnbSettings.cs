namespace Mews.ExchangeRate.Provider.CNB.Configuration;
internal class CnbSettings
{
    public static string Property => "CnbSettings";

    public string ExratesEndpoint { get; init; } = string.Empty;
    public string HealthcheckEndpoint { get; init; } = string.Empty;    
}
