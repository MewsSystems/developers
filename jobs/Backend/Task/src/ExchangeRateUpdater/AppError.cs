namespace ExchangeRateUpdater;

public class AppError(string message)
{
    public string Message { get; } = message;
}