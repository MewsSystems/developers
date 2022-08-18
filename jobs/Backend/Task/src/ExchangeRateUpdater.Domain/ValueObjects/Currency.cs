using System;
using System.Text.RegularExpressions;
using ValueOf;
namespace ExchangeRateUpdater.Domain.ValueObjects
{
    public class Currency: ValueOf<string, Currency>
    {
        private static readonly Regex ThreeLettersIsoCodeRegex = new Regex("^[A-Z]{3}$");

        protected override void Validate()
        {
            if (string.IsNullOrWhiteSpace(Value) || !IsThreeLetterIsoCode(Value))
            {
                throw new ArgumentException($"{Value} is an invalid currency code");
            }
        }

        private static bool IsThreeLetterIsoCode(string currencyCode)
        {
            return ThreeLettersIsoCodeRegex.IsMatch(currencyCode);
        }
    }
}