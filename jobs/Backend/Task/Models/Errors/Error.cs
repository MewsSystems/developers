namespace ExchangeRateUpdater.Models.Errors;

/// <summary>
/// Represents an error with an error message and error type.
/// </summary>
internal class Error
{
    public string ErrorMessage { get; private set; }
    public ErrorType ErrorType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with the specified error type.
    /// </summary>
    /// <param name="errorType">The error type.</param>
    public Error(ErrorType errorType)
    {
        ErrorType = errorType;
    }

    /// <summary>
    /// Sets the error message and returns the modified <see cref="Error"/> object.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <returns>The modified <see cref="Error"/> object.</returns>
    internal Error WithMessage(string message)
    {
        ErrorMessage = message;
        return this;
    }

    /// <summary>
    /// Returns a string representation of the error.
    /// </summary>
    /// <returns>A string representation of the error.</returns>
    public override string ToString() =>
        $"{ErrorType}: {ErrorMessage}";
}
