namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Exceptions;

/// <summary>
/// Represents errors that occur while parsing CNB exchange rate responses.
/// </summary>
public class CzechNationalBankParsingException(string message) : Exception(message)
{ }