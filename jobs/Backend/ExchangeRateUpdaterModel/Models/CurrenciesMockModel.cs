using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdaterConsole.Models
{
    internal class CurrenciesMockModel
    {
        public const string USD = "USD";
        public const string EUR = "EUR";
        public const string CZK = "CZK";
        public const string GBP = "GBP";
        public const string JPY = "JPY";
        public const string KES = "KES";
        public const string RUB = "RUB";
        public const string THB = "THB";
        public const string TRY = "TRY";

        public static List<string> GetAllCurrencies()
        {
            return typeof(CurrenciesMockModel).GetFields()
                .Where(currency => currency.IsLiteral && !currency.IsInitOnly)
                .Select(currency => currency.GetRawConstantValue()
                .ToString())
                .ToList();
        }
    }
}
