namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents an error response that contains a message describing the error.
    /// </summary>
    public class ErrorResponse(string message)
    {
        /// <summary>
        /// Gets or sets the message that describes the error.
        /// </summary>
        public string Message { get; set; } = message;
    }
}
