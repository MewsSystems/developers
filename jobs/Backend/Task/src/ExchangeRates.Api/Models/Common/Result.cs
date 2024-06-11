namespace ExchangeRates.Api.Models;

public class Result
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public ErrorType? ErrorType { get; set; }
    public Exception? Exception { get; set; }
}

public class Result<T> : Result where T : class
{
    public T? Value { get; set; }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };

    public static Result<T> Error(string errorMessage, ErrorType? errorType = null, Exception? exception = null) =>
                                    new() { IsSuccess = false, ErrorMessage = errorMessage, ErrorType = errorType, Exception = exception };
}

public enum ErrorType
{
    BadRequest = 400,
    Unauthorized = 401,
    NotFound = 404,
    Timeout = 408,
    Conflict = 409,
    Cancelled = 418,
    InternalError = 500
}