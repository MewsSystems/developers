using System.Collections.Generic;
using System.Net.Http;

namespace ExchangeRateUpdater
{
    public class CzechNationalBankService : BankServiceBase, ICzechNationalBankService
    {
        //https://publicapi.dev/czech-national-bank-api
        //Date parameter can be added in this format
        //?date={0:dd\.MM\.yyyy}
        private const string CNB_API_URL = @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        protected CzechNationalBankService(HttpClient client) : base(client) { }

        public IEnumerable<CzechNationalBankExchangeRate> GetRates()
            => Get<IEnumerable<CzechNationalBankExchangeRate>>(CNB_API_URL)
                .Result;
    }
}