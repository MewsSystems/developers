using System;

namespace ExchangeRateUpdater.Infra;

public class Result<TValue, TError>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    private TValue? Value { get; }
    private TError? Error { get; }

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }
    
    private Result(TError error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }
    
    public static Result<TValue, TError> Success(TValue value)
    {
        return new Result<TValue, TError>(value);
    }
    
    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError>(error);
    }
    
    public TResult Match<TResult>(
        Func<TValue, TResult> success,
        Func<TError, TResult> failure) => IsSuccess ? success(Value!) : failure(Error!);
}
