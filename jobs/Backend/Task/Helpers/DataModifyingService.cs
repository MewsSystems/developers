using ExchangeRateUpdater.Helpers.Interfaces;
using ExchangeRateUpdater.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Helpers
{
    public class DataModifyingService : IDataModifyingService
    {
        public Course DeserializeString(string content)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Course));
                using (var reader = new StringReader(content))
                {
                    return (Course)serializer.Deserialize(reader);
                };
            }
            catch (Exception)
            {
                //LOG
                return null;
            }
        }

        public IEnumerable<CurrencyValue> CommonCurrencies(IEnumerable<Entity> sourceCurrency, IEnumerable<Currency> availableCurrencies)
        {
            try
            {
                var commonCurrencies = new List<CurrencyValue>();
                foreach (var currency in availableCurrencies)
                {
                    foreach (var outsideCurrency in sourceCurrency)
                    {
                        if (currency.Code == outsideCurrency.Code)
                            commonCurrencies.Add(new CurrencyValue(outsideCurrency.Code, decimal.Parse(outsideCurrency.ExchangeRate.Replace(",", "."), CultureInfo.InvariantCulture)));
                    }
                }

                return commonCurrencies;
            }
            catch (Exception)
            {
                //LOG
                return null;
            }
        }
    }
}