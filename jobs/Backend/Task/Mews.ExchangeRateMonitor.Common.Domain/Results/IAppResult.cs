namespace Mews.ExchangeRateMonitor.Common.Domain.Results;

/// <summary>
/// Represents a result of an operation which encapsulates either a success or an error state.
/// </summary>
public interface IAppResult
{
    /// <summary>
    /// Gets the collection of errors associated with the operation result.
    /// </summary>
    List<Error>? Errors { get; }

    /// <summary>
    /// Indicates whether the operation result represents a success state.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result represents an error state.
    /// </summary>
    bool IsError { get; }
}

/// <summary>
/// Represents the result of an operation with the ability to define a value type
/// and encapsulate success or error states.
/// </summary>
/// <typeparam name="TValue">The type of the value returned on a successful result.</typeparam>
public interface IAppResult<out TValue> : IAppResult
{
    /// <summary>
    /// Gets the value associated with the operation result. This can represent the successful outcome of the operation.
    /// </summary>
    TValue? Value { get; }
}