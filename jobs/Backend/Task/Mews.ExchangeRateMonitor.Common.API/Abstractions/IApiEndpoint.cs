using Microsoft.AspNetCore.Builder;

namespace Mews.ExchangeRateMonitor.Common.API.Abstractions;

/// <summary>
/// Represents an interface defining the contract for mapping API endpoints into a web application.
/// </summary>
public interface IApiEndpoint
{
    /// <summary>
    /// Maps the API endpoint for the specified web application instance.
    /// </summary>
    /// <param name="app">The web application instance where the API endpoint will be mapped.</param>
    void MapEndpoint(WebApplication app);
}