using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Mappers
{
    public static class ExchangeRateMapper
    {
        private const string CodeKey = "Code";

        private const string AmountKey = "Amount";

        private const string RateKey = "Rate";

        private const string TargetCode = "CZK";

        public static IEnumerable<ExchangeRate> MapExchangeRateListFromString(IEnumerable<Currency> currencies, string rawData)
        {
            var exchangeDataList = new List<ExchangeRate>();

            if (string.IsNullOrEmpty(rawData)) return exchangeDataList;

            var dataLines = rawData.Split("\n");

            if (dataLines.Length < 3) return exchangeDataList;

            var header = dataLines[1];
            var headerValues = header.Split("|").ToList();

            var codeIndex = headerValues.IndexOf(CodeKey);
            var amountIndex = headerValues.IndexOf(AmountKey);
            var rateIndex = headerValues.IndexOf(RateKey);

            for (var i = 2; i < dataLines.Length; i++)
            {
                var exchangeRate = MapExchangeRateFromLine(currencies, dataLines[i], codeIndex, amountIndex, rateIndex);
                if (exchangeRate != null) exchangeDataList.Add(exchangeRate);
            }

            return exchangeDataList;
        }

        private static ExchangeRate? MapExchangeRateFromLine(IEnumerable<Currency> currencies, string line, int codeIndex, int amountIndex, int rateIndex)
        {
            var valueList = line.Split("|");
            if (codeIndex >= valueList.Length || amountIndex >= valueList.Length || rateIndex >= valueList.Length) return null;

            var sourceCode = valueList[codeIndex];
            var sourceCurrency = currencies.FirstOrDefault(c => c.Code == sourceCode);
            if (sourceCurrency == null) return null;

            if (!int.TryParse(valueList[amountIndex], out var amount)) return null;
            if (!decimal.TryParse(valueList[rateIndex], out var rate)) return null;

            return new ExchangeRate(new Currency(sourceCode), new Currency(TargetCode), rate / amount);
        }
    }
}
