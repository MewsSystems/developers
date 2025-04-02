using System.Globalization;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private HttpClient _client;
        private static readonly Currency _targetCurrency = new Currency("CZK");
        private static readonly string _uriTemplate = "https://www.cnb.cz/en/financial-markets/" +
            "foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/" +
            "selected.txt?from=STARTDATE&to=ENDDATE&currency=CURRENCYCODE&format=txt";
        private static readonly string _dateStringFormat = "dd.MM.yyyy";
        private static readonly int _amountIndex = 22;
        private static readonly int _rateIndex = 11;

        public ExchangeRateProvider(HttpClient client)
        {
            _client = client;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exchangeRates = new List<ExchangeRate>();

            string startDate = DateTime.Today.AddMonths(-1).ToString(_dateStringFormat);
            string endDate = DateTime.Today.ToString(_dateStringFormat);

            string Uri = _uriTemplate;
            Uri = Uri.Replace("STARTDATE", startDate);
            Uri = Uri.Replace("ENDDATE", endDate);

            foreach (var currency in currencies)
            {
                string updatedUri = Uri.Replace("CURRENCYCODE", currency.Code);
                HttpResponseMessage? response;

                try
                {
                    response = _client.GetAsync(updatedUri).Result;
                    response.EnsureSuccessStatusCode();
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine($"An error has ocurred trying to get {currency} exchange rates data.");
                    throw;
                }

                // If it is empty, currency is not available
                if (response.Content.Headers.ContentLength != 0)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    var textLines = responseBody.Split('\n').ToList();
                    textLines.RemoveAt(textLines.Count - 1); // Remove last empty line

                    decimal rate;

                    try
                    {
                        int amount = int.Parse(textLines.First().Substring(_amountIndex));
                        rate = decimal.Parse(textLines.Last().Substring(_rateIndex),
                            CultureInfo.InvariantCulture.NumberFormat) / amount;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine($"An error has ocurred trying to get the {currency} " +
                        "current rate from the obtained exchange rates data. Probably because the " +
                        "API request output data from Czech National Bank has been changed.");
                        throw;
                    }

                    var exchangeRate = new ExchangeRate(new Currency(currency.Code), _targetCurrency, rate);
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }
    }
}
