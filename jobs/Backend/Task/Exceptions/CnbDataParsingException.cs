using System;

namespace ExchangeRateUpdater.Exceptions
{
    // Exception thrown when parsing CNB exchange rate data fails.
    public class CnbDataParsingException(
        string message,
        string? rawData = null,
        int? lineNumber = null,
        Exception? innerException = null) : Exception(message, innerException)
    {
        // Raw data that failed to parse.
        public string? RawData { get; } = rawData;

        // Line number where the parsing error occurred.
        public int? LineNumber { get; } = lineNumber;
    }
}
