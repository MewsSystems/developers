using DomainLayer.Enums;

namespace DomainLayer.ValueObjects;

/// <summary>
/// Represents the result of an exchange rate fetch operation.
/// Encapsulates status, counts, and error information.
/// </summary>
public record FetchResult
{
    public FetchStatus Status { get; }
    public int RatesImported { get; }
    public int RatesUpdated { get; }
    public string? ErrorMessage { get; }
    public DateTimeOffset CompletedAt { get; }

    private FetchResult(
        FetchStatus status,
        int ratesImported,
        int ratesUpdated,
        string? errorMessage,
        DateTimeOffset completedAt)
    {
        if (ratesImported < 0)
            throw new ArgumentException("Rates imported cannot be negative.", nameof(ratesImported));

        if (ratesUpdated < 0)
            throw new ArgumentException("Rates updated cannot be negative.", nameof(ratesUpdated));

        Status = status;
        RatesImported = ratesImported;
        RatesUpdated = ratesUpdated;
        ErrorMessage = errorMessage;
        CompletedAt = completedAt;
    }

    /// <summary>
    /// Creates a successful fetch result.
    /// </summary>
    public static FetchResult Success(int ratesImported, int ratesUpdated)
    {
        return new FetchResult(
            FetchStatus.Success,
            ratesImported,
            ratesUpdated,
            null,
            DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Creates a failed fetch result.
    /// </summary>
    public static FetchResult Failure(string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(errorMessage))
            throw new ArgumentException("Error message cannot be null or empty for failed fetch.", nameof(errorMessage));

        return new FetchResult(
            FetchStatus.Failed,
            0,
            0,
            errorMessage,
            DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Creates a partial fetch result (some rates succeeded, some failed).
    /// </summary>
    public static FetchResult Partial(int ratesImported, int ratesUpdated, string warningMessage)
    {
        return new FetchResult(
            FetchStatus.PartialSuccess,
            ratesImported,
            ratesUpdated,
            warningMessage,
            DateTimeOffset.UtcNow);
    }

    /// <summary>
    /// Gets the total number of rates processed.
    /// </summary>
    public int TotalRatesProcessed => RatesImported + RatesUpdated;

    /// <summary>
    /// Checks if the fetch was successful (including partial success).
    /// </summary>
    public bool IsSuccessful => Status == FetchStatus.Success || Status == FetchStatus.PartialSuccess;

    /// <summary>
    /// Checks if the fetch completely failed.
    /// </summary>
    public bool IsFailed => Status == FetchStatus.Failed;
}
