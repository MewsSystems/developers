using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Cnb
{
    public class CnbRatesProvider : IRatesProvider
    {
        private readonly CnbApiDataSource dataSource;

        public CnbRatesProvider(CnbApiDataSource dataSource)
        {
            this.dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public IEnumerable<ExchangeRate> GetAllRates()
        {
            return new CnbDataParser()
                .ParseData(this.dataSource.FetchRatesData());
        }
    }
}