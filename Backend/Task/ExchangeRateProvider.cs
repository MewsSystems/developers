using System.Collections.Generic;
using System.Linq;
using System;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        string mainCurrencies = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?";
        string otherCurrencies = @"https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?";


        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            AppendCurrentTimeToURI();
            ExchangeRateReader rateReader = new ExchangeRateReader(mainCurrencies, currencies, "CZK");
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            // reading exchange rates from mainCurrencies webPage
            AppendExchangeRates(rateReader, exchangeRates);

            // reading exchange rates from otherCurrencies webPage
            rateReader.ChangeWebPage(otherCurrencies);
            AppendExchangeRates(rateReader, exchangeRates);

            return exchangeRates;
        }

        public void AppendExchangeRates(ExchangeRateReader rateReader, List<ExchangeRate> exchangeRates)
        {
            // appending read exchange rates
            ExchangeRate eR;
            while (rateReader.Peek() != -1)
                if ((eR = rateReader.Read()) != null)
                    exchangeRates.Add(eR);
        }

        public void AppendCurrentTimeToURI()
        {
            // Exchange rates of main currencies 
            // are updated on working days on 14:30.
            // We set up dates so that we would get
            // the latest exchange rates.
            DateTime date = DateTime.Now;
            int month = date.Month;
            if (date.DayOfWeek == DayOfWeek.Saturday)
                date = date.AddDays(-1);

            else if (date.DayOfWeek == DayOfWeek.Sunday)
                date = date.AddDays(-2);

            else if (date.Hour < 14 || (date.Hour == 14 && date.Minute <= 30))
                date = date.AddDays(-1);

            mainCurrencies += $"date={date.Day}.{date.Month}.{date.Year}";

            // Exchange rates of other currencies
            // are updated the last day of the month.
            if (month == date.Month)
                date = date.AddMonths(-1);

            otherCurrencies += $"year={date.Year}&month={date.Month}";

        }
    }
}
