using ConsoleTestApp.Clients;
using ConsoleTestApp.Config;

namespace ConsoleTestApp.Core;

/// <summary>
/// Factory for creating API clients based on protocol type.
/// </summary>
public class ApiClientFactory
{
    private readonly ApiEndpoints _endpoints;

    public ApiClientFactory(ApiEndpoints endpoints)
    {
        _endpoints = endpoints;
    }

    /// <summary>
    /// Create an API client for the specified protocol.
    /// </summary>
    public IApiClient CreateClient(ApiProtocol protocol)
    {
        return protocol switch
        {
            ApiProtocol.Rest => new RestApiClient(_endpoints),
            ApiProtocol.Soap => new SoapApiClient(_endpoints),
            ApiProtocol.Grpc => new GrpcApiClient(_endpoints),
            _ => throw new ArgumentException($"Unknown protocol: {protocol}", nameof(protocol))
        };
    }

    /// <summary>
    /// Create all API clients for comparison mode.
    /// </summary>
    public Dictionary<ApiProtocol, IApiClient> CreateAllClients()
    {
        return new Dictionary<ApiProtocol, IApiClient>
        {
            [ApiProtocol.Rest] = new RestApiClient(_endpoints),
            [ApiProtocol.Soap] = new SoapApiClient(_endpoints),
            [ApiProtocol.Grpc] = new GrpcApiClient(_endpoints)
        };
    }
}

/// <summary>
/// Supported API protocols.
/// </summary>
public enum ApiProtocol
{
    Rest,
    Soap,
    Grpc
}
