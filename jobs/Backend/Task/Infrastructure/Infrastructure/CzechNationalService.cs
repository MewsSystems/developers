using System.Globalization;
using Application;
using Domain;

namespace Infrastructure
{
    public class CzechNationalBankService : ServiceBase, ICzechNationalBankService
    {
        //https://publicapi.dev/czech-national-bank-api
        //Date parameter can be added in this format
        //?date={0:dd\.MM\.yyyy}
        private const string CnbApiUrl = @"https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        public CzechNationalBankService(HttpClient client) 
            : base(client) { }

        public IEnumerable<CzechNationalBankExchangeRate> GetRates()
            => Get<CzechNationalBankExchangeRateResponse>(CnbApiUrl)
                .Result
                .CzechNationalBankExchangeRateList
                .Rates
                .Select(MapToCzechNationalBankExchangeRate);

        private static CzechNationalBankExchangeRate MapToCzechNationalBankExchangeRate(CnbExchangeRate cnbExchangeRate)
            => new()
            {
                Amount = int.Parse(cnbExchangeRate.Amount),
                Code = cnbExchangeRate.Code,
                Country = cnbExchangeRate.Country,
                Currency = cnbExchangeRate.Currency,
                Rate = decimal.Parse(cnbExchangeRate.Rate, CultureInfo.GetCultureInfo("cs-CZ"))
            };
    }
}