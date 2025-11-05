namespace DomainLayer.Exceptions;

/// <summary>
/// Exception thrown when attempting to use a provider that is not active.
/// </summary>
public class ProviderNotActiveException : DomainException
{
    public string ProviderCode { get; }
    public string CurrentStatus { get; }

    public ProviderNotActiveException(string providerCode, string currentStatus)
        : base($"Provider '{providerCode}' is not active. Current status: {currentStatus}")
    {
        ProviderCode = providerCode;
        CurrentStatus = currentStatus;
    }

    public ProviderNotActiveException(string providerCode, string currentStatus, Exception innerException)
        : base($"Provider '{providerCode}' is not active. Current status: {currentStatus}", innerException)
    {
        ProviderCode = providerCode;
        CurrentStatus = currentStatus;
    }
}
