using System.Collections.Generic;

namespace Mews.Integrations.Cnb.Contracts.Configuration;

/// <summary>
/// Cnb integration configuration
/// </summary>
public class CnbConfiguration
{
    /// <summary>
    /// Base url of cnb-api
    /// </summary>
    public string BaseUrl { get; set; } = null!;

    /// <summary>
    /// Currency codes that will be used for exchange rate retrieval
    /// </summary>
    public List<string> Currencies { get; set; } = [];
}
