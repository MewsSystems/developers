using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CNBProvider : ICustomExchangeProvider
    {
        private const string CNB_URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        public async Task<IEnumerable<ExchangeRate>> GetData(string baseCurrencyCode, IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();

            try
            {
                var data = await HttpHelper.LoadData(CNB_URL); // načtení dat
                ParseData(data, baseCurrencyCode, currencies, exchangeRates); // zpracování dat
            }
            catch (Exception ex)
            {
                // zalogování chyby ex.ToString()
                Console.WriteLine(ex.ToString());
            }

            return exchangeRates;
        }

        private void ParseData(string data, string baseCurrencyCode, IEnumerable<Currency> currencies, List<ExchangeRate> exchangeRates)
        {
            var lines = data.Split('\n').Skip(2).Where(x => !String.IsNullOrWhiteSpace(x)); // první dva řádky jsou headery; 
            var baseCurrency = new Currency(baseCurrencyCode); // všechno děláme v poměru ke koruně

            try
            {
                foreach (var line in lines)
                {
                    var parts = line.Split('|'); // země|měna|množství|kód|kurz

                    var currencyCode = parts[3];
                    var definedCurrency = currencies.FirstOrDefault(x => x.Code.ToLower() == currencyCode.ToLower());

                    if (definedCurrency == null)
                    {
                        continue;
                    }

                    int amount = 0;
                    if (!int.TryParse(parts[2], out amount))
                    {
                        throw new FormatException($"Nepovedlo se naparsovat amount hodnotu '{parts[2]}' na int.");
                    }

                    decimal rate = 0m;
                    if (!decimal.TryParse(parts[4], out rate))
                    {
                        throw new FormatException($"Nepovedlo se naparsovat rate hodnotu '{parts[4]}' na decimal.");
                    }

                    var currency = new Currency(currencyCode);
                    var exchangeRate = new ExchangeRate(baseCurrency, currency, rate / amount); // některé měny nejsou 1:1, ale např. Filipíny|peso|100|PHP|43,861 => za 100 peso dostanu 43 korun.
                    exchangeRates.Add(exchangeRate);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Došlo k následující chybě během zpracování obsahu, zkontrolujte vstupní data. Chyba: '{ex.ToString()}'. Data: {data}.");
            }
        }
    }
}
