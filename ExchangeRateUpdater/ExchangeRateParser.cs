using System;
using System.Globalization;
using Newtonsoft.Json.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateParser : IExchangeRateParser
    {
        public decimal? ParseExchangeRateResponse(JObject response, string rootObjectKey)
        {
            if (!response.HasValues)
            {
                return null;
            }
            
            var rootElement = response.Root[rootObjectKey];
            
            if (rootElement == null)
                throw new Exception($"Exchange rate resopnse in not correct format: {response}");

            var valueElement = rootElement["val"];
            
            if (valueElement == null)
                throw new Exception($"Exchange rate resopnse in not correct format: {response}");

            var rateValue = valueElement.Value<string>();
            decimal rateParsedValue;

            if (!decimal.TryParse(rateValue, NumberStyles.AllowDecimalPoint, new NumberFormatInfo(), out rateParsedValue))
                throw new Exception($"Exchange rate value in not correct format: {rateValue}");

            return rateParsedValue;
        }
    }
}