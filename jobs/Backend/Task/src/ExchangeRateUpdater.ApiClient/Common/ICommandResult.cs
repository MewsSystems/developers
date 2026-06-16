namespace ExchangeRateUpdater.ApiClient.Common
{
    public interface IEmptyCommandResult
    {
        string ErrorMessage { get; }
        bool IsSuccess { get; }
    }

    public interface ICommandResult<T> : IEmptyCommandResult
    {
        T Payload { get; set; }
    }

}
