using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Configuration;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        private string ExchangeRateFile;
        private string ExchangeRateSourceCurrency;

        public ExchangeRateProvider()
        {
            ExchangeRateFile = ConfigurationManager.AppSettings["Exchange Rate File Path"];
            ExchangeRateSourceCurrency = ConfigurationManager.AppSettings["Exchange Rate Source Currency"];

        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies, ExchangeRateType rateType)
        {

            string[] lines = File.ReadAllText(ExchangeRateFile).Split('\n');

            var rates = new List<ExchangeRate>();

            var sourceCurrency = new Currency(ExchangeRateSourceCurrency);

            foreach (var row in lines.Skip(2))
            {
                var fields = row.Split(';');

                string currency = fields[0].Trim().ToUpper();
                if (currencies.Any(x => x.Code == currency))
                {
                    var targetCurency = new Currency(currency);


                    decimal value = 0;

                    switch (rateType)
                    {
                        case ExchangeRateType.BuyInCash:
                            {
                                decimal.TryParse(fields[1].Trim(), out value);
                                break;
                            }
                        case ExchangeRateType.SellInCash:
                            {
                                decimal.TryParse(fields[2].Trim(), out value);
                                break;
                            }

                        case ExchangeRateType.MiddleInCash:
                            {
                                decimal value1 = 0;
                                decimal value2 = 0;
                                decimal.TryParse(fields[1].Trim(), out value1);
                                decimal.TryParse(fields[2].Trim(), out value2);
                                value = (value1 + value2) / 2;
                                break;
                            }


                        case ExchangeRateType.BuyTransfer:
                            {
                                decimal.TryParse(fields[3].Trim(), out value);
                                break;
                            }
                        case ExchangeRateType.SellTransfer:
                            {
                                decimal.TryParse(fields[3].Trim(), out value);
                                break;
                            }

                        case ExchangeRateType.MiddleTransfer:
                            {
                                decimal value1 = 0;
                                decimal value2 = 0;
                                decimal.TryParse(fields[3].Trim(), out value1);
                                decimal.TryParse(fields[4].Trim(), out value2);
                                value = (value1 + value2) / 2;
                                break;
                            }

                        case ExchangeRateType.CentralBank:
                            {
                                decimal.TryParse(fields[5].Trim(), out value);
                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }

                    var rate = new ExchangeRate(sourceCurrency, targetCurency, rateType, value);
                    rates.Add(rate);
                }
            }

            return rates.ToArray();
        }
    }

   
}
