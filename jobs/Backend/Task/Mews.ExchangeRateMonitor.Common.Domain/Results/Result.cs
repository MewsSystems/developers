namespace Mews.ExchangeRateMonitor.Common.Domain.Results;


/// <summary>
/// Represents the success result of an operation, typically used to indicate that a process has completed successfully without returning a specific value or data.
/// </summary>
public readonly record struct Success;

/// <summary>
/// Represents a class containing utility methods for handling operation results.
/// </summary>
public static class Result
{
    /// <summary>
    /// Represents a successful operation result.
    /// </summary>
    public static Success Success => default;
}

public readonly partial record struct Result<TValue> : IAppResult<TValue>
{
    private readonly TValue? _value = default;

    private Result(TValue value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        _value = value;
    }

    private Result(Error error)
    {
        Errors = [error];
    }

    private Result(List<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors);

        if (errors.Count == 0)
        {
            throw new ArgumentException("Cannot create an Result<TValue> from an empty collection of errors. Provide at least one error.", nameof(errors));
        }

        Errors = errors;
    }

    /// <summary>
    /// Gets a value indicating whether the state is a success.
    /// </summary>
    public bool IsSuccess => Errors is null or [];

    /// <summary>
    /// Gets a value indicating whether the state is error.
    /// </summary>
    public bool IsError => Errors.Count > 0;

    /// <summary>
    /// Gets the collection of errors.
    /// </summary>
    public List<Error> Errors { get; } = [];

    /// <summary>
    /// Gets the value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no value is present.</exception>
    public TValue? Value
    {
        get
        {
            if (IsError)
            {
                throw new InvalidOperationException("The Value property cannot be accessed when Errors property is not empty. Check IsSuccess or IsError before accessing the Value.");
            }

            return _value;
        }
    }

    /// <summary>
    /// Gets the first error.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when no errors are present.</exception>
    public Error FirstError
    {
        get
        {
            if (!IsError)
            {
                throw new InvalidOperationException("The FirstError property cannot be accessed when Errors property is empty. Check IsError before accessing FirstError.");
            }

            return Errors[0];
        }
    }
}