﻿namespace ExchangeRateUpdater.Domain.Model.Cnb.Rs
{
    public class CnbExchangeRatesRsModel
    {
        public CnbExchangeRatesRsModelRate[] Rates { get; set; }

        public class CnbExchangeRatesRsModelRate
        {
            public int Amount { get; set; }
            public string CurrencyCode { get; set; }
            public decimal Rate { get; set; }
        }

    }
}
