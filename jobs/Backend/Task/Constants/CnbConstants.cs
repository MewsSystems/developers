namespace ExchangeRateUpdater.Constants;

/// <summary>
/// Constants defining the Czech National Bank data format and business rules
/// </summary>
public static class CnbConstants
{
    /// <summary>
    /// The base currency code for Czech National Bank
    /// </summary>
    public const string CurrencyCode = "CZK";

    /// <summary>
    /// Expected date format in CNB data files
    /// </summary>
    public const string DateFormat = "d MMM yyyy";

    /// <summary>
    /// Minimum number of lines expected in a valid CNB response
    /// </summary>
    public const int ExpectedMinimumLines = 3;

    /// <summary>
    /// Minimum number of space-separated parts expected in the date line for parsing
    /// </summary>
    public const int MinimumDateParts = 3;

    /// <summary>
    /// Number of fields expected in each exchange rate line
    /// </summary>
    public const int ExpectedFieldCount = 5;

    /// <summary>
    /// Field separator character used in CNB data format
    /// </summary>
    public const char FieldSeparator = '|';
}
