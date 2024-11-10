namespace ExchangeRate.Api.Configuration.Settings;

public sealed record TelemetrySettings(
    string TelemetryEndpointUrl,
    string TelemetryEndpointApiKey,
    bool TelemetryEndpointUseAuthentication);