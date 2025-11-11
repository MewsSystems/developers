namespace DomainLayer.Exceptions;

/// <summary>
/// Exception thrown when a provider has been quarantined due to consecutive failures.
/// </summary>
public class ProviderQuarantinedException : DomainException
{
    public string ProviderCode { get; }
    public int ConsecutiveFailures { get; }

    public ProviderQuarantinedException(string providerCode, int consecutiveFailures)
        : base($"Provider '{providerCode}' has been quarantined after {consecutiveFailures} consecutive failures. Manual intervention required.")
    {
        ProviderCode = providerCode;
        ConsecutiveFailures = consecutiveFailures;
    }

    public ProviderQuarantinedException(string providerCode, int consecutiveFailures, Exception innerException)
        : base($"Provider '{providerCode}' has been quarantined after {consecutiveFailures} consecutive failures. Manual intervention required.", innerException)
    {
        ProviderCode = providerCode;
        ConsecutiveFailures = consecutiveFailures;
    }
}
