namespace ExchangeRateUpdater.ExchangeRateProviders;

public class Result<TValue>
{
    public bool HasSucceeded { get; }
    public string ErrorMessage { get; }
    public TValue Value { get; }

    protected Result(bool hasSucceeded, string errorMessage, TValue value)
    {
        HasSucceeded = hasSucceeded;
        ErrorMessage = errorMessage;
        Value = value;
    }

    public static implicit operator TValue(Result<TValue> result) => result.Value;

    public static Result<TValue> Succeeded(TValue value)
    {
        return new Result<TValue>(true, null, value);
    }

    public static Result<TValue> Failed(string errorMessage)
    {
        return new Result<TValue>(false, errorMessage, default);
    }
}