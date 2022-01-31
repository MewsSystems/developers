using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using NLog;

namespace ExchangeRateUpdater.Sources
{
    public class ExchangeRateParserCzechBank : IExchangeRateParser
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Regex _exchangeRateRegex;

        public ExchangeRateParserCzechBank(IConfiguration configuration)
        {
            _exchangeRateRegex = new Regex(configuration["ExchangeRateRegex"]);
        }
        public ExchangeRateParseResult ParseExchangeRate(string text)
        {
            try
            {
                var match = _exchangeRateRegex.Match(text);
                if (!match.Success)
                {
                    Logger.Warn($"Unable to parse exchange rate from: '{text}'");
                    return new ExchangeRateParseResult(false, null);
                }
                var amount = ParseAmount(match.Groups["amount"].Value);
                var rate = ParseRate(match.Groups["rate"].Value);
                
                var exchangeRate = new ExchangeRate(
                    new Currency("CZK"),
                    new Currency(match.Groups["code"].Value),
                    NormalizeRate(amount, rate)
                );
                return new ExchangeRateParseResult(true, exchangeRate);
            }
            catch (Exception e)
            {
                Logger.Fatal($"Failed to parse exchange rate: {e.Message}", e);
                throw;
            }
        }

        public int ParseAmount(string amount) => int.Parse(amount);
        public decimal NormalizeRate(int amount, decimal rate) => rate / amount;

        public decimal ParseRate(string rate,
                                NumberStyles numberStyles = NumberStyles.Any,
                                CultureInfo culture = null) => 
            decimal.Parse(rate, numberStyles, culture ?? CultureInfo.InvariantCulture);
    }
}
