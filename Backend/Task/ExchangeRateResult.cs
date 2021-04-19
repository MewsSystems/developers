namespace ExchangeRateUpdater
{
    public class ExchangeRateResult
    {

        public ExchangeRateResult(bool success, ExchangeRate rate, string message)
        {
            Success = success;
            Rate = rate;
            Message = message;
        }

        public bool Success { get; }
        public ExchangeRate Rate { get; }
        public string Message { get; }
    }
}
