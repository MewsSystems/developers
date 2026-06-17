namespace ExchangeRateData.DataConnectors.Models
{
    /// <summary>
    /// Data model of ExchangeRate
    /// </summary>
    public class ExchangeRate
    {
        public ExchangeRate(string code)
        {

        }
        public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Value = value;
        }

        /// <summary>
        /// The source currency of exchange rate
        /// </summary>
        public Currency SourceCurrency { get; set; }

        /// <summary>
        /// The target currency of exchange rate
        /// </summary>
        public Currency TargetCurrency { get; }

        /// <summary>
        /// The value of exchange rate
        /// </summary>
        public decimal Value { get; }


        public override string ToString()
        {
            return $"{SourceCurrency}/{TargetCurrency}={Value}";
        }

        public string _resultRate;
        /// <summary>
        /// Custom property for json result (if needed, flag [IgnoreProperty] can be used for other properties when serializing)
        /// </summary>
        public string resultRate
        {
            get { return $"{SourceCurrency}/{TargetCurrency}={Value}"; }
            set { _resultRate = $"{SourceCurrency}/{TargetCurrency}={Value}"; }
        }


        /// <summary>
        /// Converts data from txt to exchange rate data model
        /// </summary>
        /// <param name="data">Data to convert</param>
        /// <param name="inputCurrencies">Currencies user want to see</param>
        /// <returns>List of ExchangeRate</returns>
        public static List<ExchangeRate>? ConvertDataToExchangeRate(string data, string[] inputCurrencies)
        {
            List<ExchangeRate> listOfRates = new List<ExchangeRate>();

            //check if currencies in input are permitted
            IEnumerable<Currency> availableCurrencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("JPY"),
                new Currency("KES"),
                new Currency("RUB"),
                new Currency("THB"),
                new Currency("TRY"),
                new Currency("XYZ")
            };

            List<Currency> matchingCurrencies = availableCurrencies
                .Where(availableCurrency => inputCurrencies.Contains(availableCurrency.Code))
                .ToList();


            //split data from third party
            var split = data.Split(new Char[] { '#', '\r', '\n' },
                StringSplitOptions.RemoveEmptyEntries);

            //going through cycle and assigning values to model properties
            for (int i = 0; i < split.Length; i++)
            {
                //ignoring date and time
                if (i == 0)
                {
                    continue;
                }

                if (i != 1 && i != 2)
                {

                    var item = split[i].Split('|');

                    if (item.Length != 5)
                        continue;


                    bool success = decimal.TryParse(item[4], out decimal rate);
                    {
                        if (success)
                        {
                            ExchangeRate exxR = new ExchangeRate(new Currency(item[3]), new Currency("CZK"), rate);
                            if (matchingCurrencies.Any(x => x.Code == exxR.SourceCurrency.ToString()))
                            {
                                listOfRates.Add(exxR);
                            }
                        }
                    }

                }
            }

            return listOfRates;
        }
    }
}
