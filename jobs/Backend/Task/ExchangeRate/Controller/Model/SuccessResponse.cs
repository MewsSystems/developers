namespace ExchangeRateUpdater.ExchangeRate.Controller.Model
{
    /// <summary>
    /// Represents a success response that contains a message and additional data.
    /// </summary>
    /// <typeparam name="T">The type of the data included in the response.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SuccessResponse{T}"/> class.
    /// </remarks>
    /// <param name="message">The success message.</param>
    /// <param name="data">The additional data.</param>
    public class SuccessResponse<T>(string message, T data)
    {

        /// <summary>
        /// Gets or sets the success message.
        /// </summary>
        public string Message { get; set; } = message;

        /// <summary>
        /// Gets or sets the additional data.
        /// </summary>
        public T Data { get; set; } = data;
    }
}
