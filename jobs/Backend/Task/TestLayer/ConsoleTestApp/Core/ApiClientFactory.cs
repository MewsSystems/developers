using ConsoleTestApp.Clients;
using ConsoleTestApp.Config;

namespace ConsoleTestApp.Core;

/// <summary>
/// Factory for creating API clients based on protocol type.
/// Maintains singleton instances to preserve authentication state.
/// </summary>
public class ApiClientFactory
{
    private readonly ApiEndpoints _endpoints;
    private readonly Dictionary<ApiProtocol, IApiClient> _clients;

    public ApiClientFactory(ApiEndpoints endpoints)
    {
        _endpoints = endpoints;
        _clients = new Dictionary<ApiProtocol, IApiClient>
        {
            [ApiProtocol.Rest] = new RestApiClient(_endpoints),
            [ApiProtocol.Soap] = new SoapApiClient(_endpoints),
            [ApiProtocol.Grpc] = new GrpcApiClient(_endpoints)
        };
    }

    /// <summary>
    /// Get the API client for the specified protocol.
    /// Returns the same instance for subsequent calls to preserve authentication state.
    /// </summary>
    public IApiClient CreateClient(ApiProtocol protocol)
    {
        if (!_clients.TryGetValue(protocol, out var client))
        {
            throw new ArgumentException($"Unknown protocol: {protocol}", nameof(protocol));
        }

        return client;
    }

    /// <summary>
    /// Get all API clients for comparison mode.
    /// </summary>
    public Dictionary<ApiProtocol, IApiClient> CreateAllClients()
    {
        return _clients;
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
