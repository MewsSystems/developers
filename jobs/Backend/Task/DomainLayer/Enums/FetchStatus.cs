namespace DomainLayer.Enums;

/// <summary>
/// Represents the status of an exchange rate fetch operation.
/// </summary>
public enum FetchStatus
{
    /// <summary>
    /// Fetch operation is currently in progress.
    /// </summary>
    Running = 1,

    /// <summary>
    /// Fetch operation completed successfully.
    /// </summary>
    Success = 2,

    /// <summary>
    /// Fetch operation failed completely.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Fetch operation completed with some rates imported but encountered errors.
    /// </summary>
    PartialSuccess = 4
}
