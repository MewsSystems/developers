namespace ExchangeRateUpdater.Api.Models;

public class ApiResponseBuilder
{
    private bool _success;
    private string _message = string.Empty;
    private List<string> _errors = new();
    private DateTime _timestamp = DateTime.UtcNow;

    public ApiResponseBuilder WithSuccess(bool success = true)
    {
        _success = success;
        return this;
    }

    public ApiResponseBuilder WithMessage(string message)
    {
        _message = message;
        return this;
    }

    public ApiResponseBuilder WithErrors(params string[] errors)
    {
        _errors.AddRange(errors);
        return this;
    }

    public ApiResponseBuilder WithTimestamp(DateTime timestamp)
    {
        _timestamp = timestamp;
        return this;
    }

    public ApiResponse Build()
    {
        return new ApiResponse
        {
            Success = _success,
            Message = _message,
            Errors = _errors,
            Timestamp = _timestamp
        };
    }

    public ApiResponse<T> Build<T>(T data)
    {
        return new ApiResponse<T>
        {
            Success = _success,
            Message = _message,
            Data = data,
            Errors = _errors,
            Timestamp = _timestamp
        };
    }

    // Convenience static methods for common scenarios
    public static ApiResponse Success(string message = "Operation completed successfully")
        => new ApiResponseBuilder().WithSuccess().WithMessage(message).Build();

    public static ApiResponse<T> Success<T>(T data, string message = "Operation completed successfully")
        => new ApiResponseBuilder().WithSuccess().WithMessage(message).Build(data);

    public static ApiResponse BadRequest(string message, params string[] errors)
        => new ApiResponseBuilder().WithSuccess(false).WithMessage(message).WithErrors(errors).Build();

    public static ApiResponse NotFound(string message, params string[] errors)
        => new ApiResponseBuilder().WithSuccess(false).WithMessage(message).WithErrors(errors).Build();

    public static ApiResponse InternalError(string message = "An unexpected error occurred")
        => new ApiResponseBuilder().WithSuccess(false).WithMessage(message).Build();
}