using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExchangeRateUpdater
{
    public static class Constants
    {
        public const string AmountHeaderName = "Amount";
        public const string CodeHeaderName = "Code";
        public const string RateHeaderName = "Rate";

        public static ReadOnlyCollection<string> ExchangeRateColumnNames =>
            new List<string>() { AmountHeaderName, CodeHeaderName, RateHeaderName }.AsReadOnly();
    }
}