namespace InfrastructureLayer.ExternalServices.Adapters.Models;

/// <summary>
/// Response from a provider adapter when fetching exchange rates.
/// </summary>
public class ProviderRateResponse
{
    public bool IsSuccess { get; }
    public List<ProviderRate> Rates { get; }
    public string? ErrorMessage { get; }

    private ProviderRateResponse(bool isSuccess, DateOnly validDate, List<ProviderRate> rates, string? errorMessage)
    {
        IsSuccess = isSuccess;
        Rates = rates ?? new List<ProviderRate>();
        ErrorMessage = errorMessage;
    }

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    public static ProviderRateResponse Success(DateOnly validDate, List<ProviderRate> rates)
    {
        return new ProviderRateResponse(true, validDate, rates, null);
    }

    /// <summary>
    /// Creates a failure response.
    /// </summary>
    public static ProviderRateResponse Failure(string errorMessage)
    {
        return new ProviderRateResponse(false, default, new List<ProviderRate>(), errorMessage);
    }
}
