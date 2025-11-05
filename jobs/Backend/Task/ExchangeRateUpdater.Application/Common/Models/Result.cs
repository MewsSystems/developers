namespace ExchangeRateUpdater.Application.Common.Models;

public class Result<T>
{
    public Result()
    {
        
    }
    private Result(T value, bool succeeded, string errorMessage)
    {
        Value = value;
        Succeeded = succeeded;
        Error = errorMessage;
    }

    private Result(T value, bool succeeded, IEnumerable<string> errors)
    {
        Value = value;
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }
    
    public T Value { get; private set; }
    public bool Succeeded { get; private set; }

    public string[] Errors { get; private set; }
    public string Error { get; private set; }

    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, new string[] { });
    }

    public static Result<T> Failure(string errorMessage)
    {
        return new Result<T>(default(T), false, errorMessage);
    }
    
    public static Result<T> Failure(IEnumerable<string> errorMessages)
    {
        return new Result<T>(default(T), false, errorMessages);
    }
}