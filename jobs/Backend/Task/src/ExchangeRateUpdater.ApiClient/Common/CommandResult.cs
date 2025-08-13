using System.Net;

namespace ExchangeRateUpdater.ApiClient.Common
{
    public abstract class EmptyCommandResult : IEmptyCommandResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public EmptyCommandResult WithErrors(params string[] errors)
        {
            ErrorMessage = string.Join(",", errors);
            return this;
        }
    }

    public abstract class CommandResult<T, TError> : EmptyCommandResult, ICommandResult<T>
    {
        public T Payload { get; set; }
        public TError Error { get; set; }

        public CommandResult<T, TError> WithPayload(T payload)
        {
            Payload = payload;
            ErrorMessage = string.Empty;
            return this;
        }

        public CommandResult<T, TError> WithError(TError error)
        {
            Error = error;
            return this;
        }
    }

}
