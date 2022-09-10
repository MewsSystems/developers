using Application.Services.Extensions;
using Application.Services.Interfaces;
using Domain.Core;
using Domain.Model;

namespace Application.Services.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ICNBGateway _gateway;

        public ExchangeRateProvider(ICNBGateway gateway)
        {
            _gateway = gateway;
        }

        public void LoadData()
        {
            ExchangeRateCache.Data.Clear();
            var rawdata = _gateway.LoadDataFromServer();
            var data = rawdata.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in data)
            {
                if (!line.IsToIgnoreLine())
                {
                    var exchangeRate = line.ParseExchangeRate();
                    if (exchangeRate != null)
                    {
                        if (ExchangeRateCache.Data.ContainsKey("CZK"))
                        {
                            ExchangeRateCache.Data["CZK"].Add(exchangeRate);
                        }
                        else
                        {
                            ExchangeRateCache.Data.Add("CZK", new List<ExchangeRate> { exchangeRate });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (var currency in currencies.ToList())
            {
                var exchangeRate = ExchangeRateCache.Data["CZK"].Where(x => x.SourceCurrency.Code == currency.Code).FirstOrDefault();
                if (exchangeRate != null)
                {
                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }

        public ExchangeRate? Convert(Currency sourceCurrency, Currency targetCurrency, int amount = 1, string referenceCurrencyCode = "CZK")
        {
            try
            {
                if (ExchangeRateCache.Data.ContainsKey(sourceCurrency.Code))
                {
                    decimal value = FromReferenceCurrencyCode(targetCurrency.Code, sourceCurrency.Code);
                    return new ExchangeRate(sourceCurrency, targetCurrency, amount * value);
                }

                if (ExchangeRateCache.Data.ContainsKey(targetCurrency.Code))
                {
                    decimal value = ToReferenceCurrencyCode(sourceCurrency.Code, targetCurrency.Code);
                    return new ExchangeRate(sourceCurrency, targetCurrency, amount * value);
                }

                decimal sourceToReferenceCurrencyValue = ToReferenceCurrencyCode(sourceCurrency.Code, referenceCurrencyCode);
                decimal targetToReferenceCurrencyValue = ToReferenceCurrencyCode(targetCurrency.Code, referenceCurrencyCode);

                if (sourceToReferenceCurrencyValue == 0 || targetToReferenceCurrencyValue == 0)
                    return null;

                var total = amount * (sourceToReferenceCurrencyValue / targetToReferenceCurrencyValue);
                return new ExchangeRate(sourceCurrency, targetCurrency, total);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("One of the currencies that you want to convert is not known");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex}");
                return null;
            }
        }

        //FROM CZK
        private decimal FromReferenceCurrencyCode(string currencyCode, string referenceCurrencyCode)
        {
            var exchangeRate = ExchangeRateCache.Data[referenceCurrencyCode].Where(x => x.SourceCurrency.Code == currencyCode).FirstOrDefault();
            if (exchangeRate == null) 
                return 0;
            return 1 / exchangeRate.Value;
           
        }

        //TO CZK
        private decimal ToReferenceCurrencyCode(string currencyCode, string referenceCurrencyCode)
        {
            var exchangeRate = ExchangeRateCache.Data[referenceCurrencyCode].Where(x => x.SourceCurrency.Code == currencyCode).FirstOrDefault();

            if (exchangeRate == null)
                return 0;

            return exchangeRate.Value;
        }

    }
}