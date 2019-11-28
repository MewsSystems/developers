using System;
using System.Globalization;

namespace ExchangeRateUpdater.CnbProvider
{
    interface ICnbFxRateRowParser
    {
        ExchangeRate ParseRateRow(string rateAsString);
    }

    class CnbFxRateRowParser : ICnbFxRateRowParser
    {
        private const char Separator = '|';

        private static class ColumnIndex
        {
            public const int Country = 0;
            public const int Name = 1;
            public const int Amount = 2;
            public const int Code = 3;
            public const int Value = 4;
        }

        public ExchangeRate ParseRateRow(string rateAsString)
        {
            if (string.IsNullOrWhiteSpace(rateAsString)) return null; 

            try
            {
                var rateStringSplit = rateAsString.Split(Separator);
                var cnbRate = new CnbRate(rateStringSplit[ColumnIndex.Code], rateStringSplit[ColumnIndex.Value], rateStringSplit[ColumnIndex.Amount]);

                return new ExchangeRate(
                    sourceCurrency: CnbExchangeRatesProvider.ProviderCurrency, 
                    targetCurrency: new Currency(cnbRate.Code),
                             value: cnbRate.Value / cnbRate.Amount);
            }
            catch (Exception e)
            {
                //log rateAsString, e
                return null;
            }
        }

        private struct CnbRate
        {
            public CnbRate(string code, string value, string amount)
            {
                Code = code;
                Amount = int.Parse(amount);
                Value = decimal.Parse(value, NumberStyles.AllowDecimalPoint);
            }

            public string Code { get; }

            public decimal Value { get; }

            public int Amount { get; }
        }
    }
}