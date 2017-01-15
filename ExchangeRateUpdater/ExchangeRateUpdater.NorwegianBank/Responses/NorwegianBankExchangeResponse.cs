using ExchangeRateUpdater.Domain;
using System;

namespace ExchangeRateUpdater.NorwegianBank.Responses
{
    /// <summary>
    /// Response currency data from Norwegian Bank
    /// </summary>
    /// <remarks>
    /// JSON format:
    /// [{
    /// 	"Id": "USD",
    /// 	"TitleNO": "Amerikanske dollar (USD)",
    /// 	"TitleEN": "US dollar (USD)",
    /// 	"CurrentValue": "8.4964"
    /// }, {
    /// 	"Id": "AUD",
    /// 	"TitleNO": "Australske dollar (AUD)",
    /// 	"TitleEN": "Australian dollar (AUD)",
    /// 	"CurrentValue": "6.3677"
    /// }]
    /// </remarks>
    class NorwegianBankExchangeResponse : IExhangeRate
    {
        /// <summary>
        /// Code of the currency
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name of the currency
        /// </summary>
        public string TitleEN { get; set; }
        /// <summary>
        /// Current exchange rate to NOK
        /// </summary>
        public decimal CurrentValue { get; set; }

        public ExchangeRate ExchangeRate {
            get
            {
                if (string.IsNullOrWhiteSpace(this.Id))
                    throw new ArgumentNullException("NorwegianBankExchangeResponse.Id is null");

                return new ExchangeRate(new Currency(this.Id), new Currency("NOK"), this.CurrentValue);
            }
        }        
    }
}
