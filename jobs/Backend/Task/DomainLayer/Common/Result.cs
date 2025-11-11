namespace DomainLayer.Common;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// Provides an alternative to exceptions for expected failures.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    protected Result(bool isSuccess, string? error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException("A successful result cannot have an error.");
        if (!isSuccess && error == null)
            throw new InvalidOperationException("A failed result must have an error.");

        IsSuccess = isSuccess;
        Error = error;
    }

    protected Result(bool isSuccess, Common.Error? error)
        : this(isSuccess, error?.ToString())
    {
    }

    public static Result Success() => new(true, (string?)null);
    public static Result Failure(string error) => new(false, error);
    public static Result Failure(Common.Error error) => new(false, error);

    public static Result<T> Success<T>(T value) => new(true, value, (string?)null);
    public static Result<T> Failure<T>(string error) => new(false, default, error);
    public static Result<T> Failure<T>(Common.Error error) => new(false, default, error);
}

/// <summary>
/// Represents the result of an operation that returns a value on success.
/// </summary>
/// <typeparam name="T">The type of the value returned on success</typeparam>
public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(bool isSuccess, T? value, string? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    protected internal Result(bool isSuccess, T? value, Common.Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    // Static factory methods that hide base class methods
    public new static Result<T> Success(T value) => new(true, value, (string?)null);
    public new static Result<T> Failure(string error) => new(false, default, error);
    public new static Result<T> Failure(Common.Error error) => new(false, default, error);
}
