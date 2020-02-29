using Microsoft.Extensions.Configuration;
using System;

namespace ExchangeRateUpdater.Configuration
{
    public class CnbExchangeRateFixingConfiguration
    {
        public CnbExchangeRateFixingConfiguration(IConfiguration configuration)
        {
            MainCurrenciesListUrl = configuration["CnbExchangeRateFixing:MainCurrenciesListUrl"];
            OtherCurrenciesListUrl = configuration["CnbExchangeRateFixing:OtherCurrenciesListUrl"];
            DataStartLine = Convert.ToInt32(configuration["CnbExchangeRateFixing:DataStartLine"]);
            CountyColumnId = Convert.ToInt32(configuration["CnbExchangeRateFixing:CountyColumnId"]);
            CurrencyColumnId = Convert.ToInt32(configuration["CnbExchangeRateFixing:CurrencyColumnId"]);
            AmountColumnId = Convert.ToInt32(configuration["CnbExchangeRateFixing:AmountColumnId"]);
            CodeColumnId = Convert.ToInt32(configuration["CnbExchangeRateFixing:CodeColumnId"]);
            RateColumnId = Convert.ToInt32(configuration["CnbExchangeRateFixing:RateColumnId"]);
            ColumnSeparator = configuration["CnbExchangeRateFixing:ColumnSeparator"];
            SourceCurrency = configuration["CnbExchangeRateFixing:SourceCurrency"];
            RetryAttempts = Convert.ToInt32(configuration["CnbExchangeRateFixing:RetryAttempts"]);
            RetryTimeout = Convert.ToInt32(configuration["CnbExchangeRateFixing:RetryTimeout"]);            
        }

        public string MainCurrenciesListUrl { get; }
        public string OtherCurrenciesListUrl { get; }
        public int DataStartLine { get; }
        public int CountyColumnId { get; }
        public int CurrencyColumnId { get; }
        public int AmountColumnId { get; }
        public int CodeColumnId { get; }
        public int RateColumnId { get; }
        public string ColumnSeparator { get; }
        public string SourceCurrency { get; }
        public int RetryAttempts { get; }
        public int RetryTimeout { get; }
    }
}
