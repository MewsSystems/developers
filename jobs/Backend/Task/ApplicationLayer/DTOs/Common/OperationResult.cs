namespace ApplicationLayer.DTOs.Common;

/// <summary>
/// Represents the result of an operation with success/failure status.
/// </summary>
public class OperationResult
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public IDictionary<string, string[]>? Errors { get; set; }

    public static OperationResult Success(string? message = null)
    {
        return new OperationResult
        {
            IsSuccess = true,
            Message = message
        };
    }

    public static OperationResult Failure(string message, IDictionary<string, string[]>? errors = null)
    {
        return new OperationResult
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}

/// <summary>
/// Represents the result of an operation that returns data.
/// </summary>
public class OperationResult<T> : OperationResult
{
    public T? Data { get; set; }

    public static OperationResult<T> Success(T data, string? message = null)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message
        };
    }

    public new static OperationResult<T> Failure(string message, IDictionary<string, string[]>? errors = null)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors
        };
    }
}
