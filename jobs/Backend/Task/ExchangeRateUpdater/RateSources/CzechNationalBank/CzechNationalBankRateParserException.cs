using System;

namespace ExchangeRateUpdater.RateSources.CzechNationalBank;

public class CzechNationalBankRateParserException : Exception
{
    public CzechNationalBankRateParserException(string err, string problemLine, string fullText) : base($"Could not parse the rate from Czech National Bank. Error: {err}. [FailedLine = {problemLine}, FullText = {fullText}]") { }
}
