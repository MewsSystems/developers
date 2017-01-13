using ExchangeRateUpdater.Domain;
using System;

namespace ExchangeRateUpdater.NorwegianBank
{
    //{"Id":"USD","TitleNO":"Amerikanske dollar (USD)","TitleEN":"US dollar (USD)","CurrentValue":"8.4863"}
    class NorwegianBankExchangeResponse
    {
        public string Id { get; set; }
        public string TitleEN { get; set; }
        public decimal CurrentValue { get; set; }

        //TODO: extract interface
        public ExchangeRate ToExchangeRate()
        {
            if (string.IsNullOrWhiteSpace(this.Id))
                throw new ArgumentNullException("NorwegianBankExchangeResponse.Id is null");

            return new ExchangeRate(new Currency("NOK"), new Currency(this.Id), this.CurrentValue);
        }
    }
}
