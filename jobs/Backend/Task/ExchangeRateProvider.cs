using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Xml.Linq;
using System.Xml;
using System.Globalization;





namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        // Czech National Bank XML endpoint for daily exchange rates
        private static readonly string BankApiUrl = 
            "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        
        
        private const int TimeoutSeconds = 10;
        private const string TargetCurrency = "CZK";

        private static readonly HttpClient _httpClient;
        
        static ExchangeRateProvider()
        {
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(TimeoutSeconds);
        }



        /// <summary>
        /// This method returns exchange rates among the specified currencies that are defined by the source.
        /// Provides exchange rates from Czech National Bank's public API.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
            {
                throw new ArgumentNullException(nameof(currencies), "Currency collection cannot be null");
            }
            var currencyList = currencies.ToList();
            if (!currencyList.Any())
            {
                return Enumerable.Empty<ExchangeRate>(); 
            }
            
            var exchangeRatesDict = FetchandParseBankData();
            return GetRequestedCurrenciesExchangeRate(exchangeRatesDict, currencies);
        }
        

        private List<ExchangeRate> GetRequestedCurrenciesExchangeRate(Dictionary<string, decimal> allExchangeRates, IEnumerable<Currency> requestedCurrencies)
        {
            var requestedCurrenciesExchangeRate = new List<ExchangeRate>();
            // Used to keep track of the processed currencies and prevent duplicates
            var seenCurrencies = new HashSet<string>(); 

            foreach (var currency in requestedCurrencies)
            {
                if (currency == null)
                {
                    continue;
                }

                string sourceCurrency = currency.Code;

                if (!seenCurrencies.Add(sourceCurrency))
                {
                    continue;
                }
                
                if (allExchangeRates.ContainsKey(sourceCurrency))
                {
                    requestedCurrenciesExchangeRate.Add(new ExchangeRate(
                        new Currency(sourceCurrency),
                        new Currency(TargetCurrency),
                        allExchangeRates[sourceCurrency]
                    ));
                }
            }
            return requestedCurrenciesExchangeRate;

        }

        private Dictionary<string, decimal> FetchandParseBankData()
        {
            try{

            LogInfo("Fetching exchange rates provided currencies");

            var response = _httpClient.GetAsync(BankApiUrl).Result;
            response.EnsureSuccessStatusCode();
            var xmlBankData = response.Content.ReadAsStringAsync().Result;

            var parsedXmlData = XDocument.Parse(xmlBankData);
            
            Dictionary<string, decimal> exchangeRates = parsedXmlData.Descendants("radek")
            .Where(x => x.Attribute("kod") != null && 
                        x.Attribute("kurz") != null && 
                        x.Attribute("mnozstvi") != null)
            .ToDictionary(
                x => x.Attribute("kod").Value,
                x => 
                {
                    // // Use Czech culture for decimal parsing
                    decimal kurz = decimal.Parse(x.Attribute("kurz").Value, CultureInfo.GetCultureInfo("cs-CZ"));
                    decimal mnozstvi = decimal.Parse(x.Attribute("mnozstvi").Value);
                    if (mnozstvi == 0)
                    {
                        throw new InvalidOperationException($"Invalid mnozstvi (0) for currency {x.Attribute("kod").Value}");
                    }
                    return kurz / mnozstvi;
                }
            );
            return exchangeRates;
            }
             catch (XmlException ex)
            {
                LogError("Failed to parse XML from the Bank", ex);
                throw;
            }
            catch (FormatException ex)
            {
                LogError("Invalid number format in Bank data", ex);
                throw;
            }
            catch (Exception ex)
            {
                LogError("Failed to fetch and parse CNB data", ex);
                throw;
            }
        }

    private void LogError(string message, Exception ex = null)
    {
        Console.Error.WriteLine($"[ERROR] {message}");
        if (ex != null)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
        }
    }

    private void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] {message}");
    }

    }
}