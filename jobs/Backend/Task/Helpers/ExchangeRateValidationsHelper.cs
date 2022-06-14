using System;
using System.Text.RegularExpressions;

public class ExchangeRateValidationsHelper : IExchangeRateValidationsHelper
{

    public bool ValidateCurrency(string currency, Settings settings)
    {
        if (currency.Length != 3)
        {
            Console.WriteLine("Currency codes should be 3 characters (ISO 4217)");
            return false;

        }
        if (!Regex.IsMatch(currency, @"^[a-zA-Z]+$"))
        {
            Console.WriteLine("Currency should consist only of letters");
            return false;

        }

        if (settings.SupportedCurrencies.Contains(currency))
        {
            return true;
        }

        Console.WriteLine($"Currency {currency} is not supported/does not exist");
        return false;
    }
}