using ExchangeRateProvider.BusinessLogic.IBusinessLogic;
using ExchangeRateProvider.DomainEntities;
using ExchangeRateProvider.DomainEntities.Constant;
using ExchangeRateProvider.DomainEntities.DTOs;
using ExchangeRateProvider.Persistence.IRepo;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace ExchangeRateProvider.BusinessLogic
{
    public class CurrencyPairRates : ICurrencyPairRates
    {
        private readonly ICurrencyExchangeRepo _currencyExchangeRepo;
        private IConfiguration _configuration;
        public CurrencyPairRates(ICurrencyExchangeRepo currencyExchangeRepo,
            IConfiguration configuration)
        {
            _currencyExchangeRepo = currencyExchangeRepo;
            _configuration = configuration;
        }

        public async Task<RatesDTO> GetAllAsync(RequestModel requestModel)
        {

            if (requestModel.DateTime == DateTime.MinValue)
                requestModel.DateTime = DateTime.Now;

            if (string.IsNullOrEmpty(requestModel.Language))
                requestModel.Language = Languages.ENGLISH;

            string path = _configuration["CzechNationalBankApiProperties:DailyRatesApiPath"];

            string uri = createUri(path, requestModel);

            string json = await _currencyExchangeRepo.GetPairsAsync(uri);

            var rates = formatRates(deserialize(json));

            return rates;
        }

        private string createUri(string path, RequestModel requestModel)
        {
            return 
                $"{path}?date={requestModel.DateTime.ToString("yyyy-MM-dd")}&lang={requestModel.Language.ToUpper()}";
        }

        private Rate deserialize(string json)
        {
            return JsonConvert.DeserializeObject<Rate>(json);
        }

        private RatesDTO formatRates(Rate rates)
        {
            var ratesDTO = new RatesDTO()
            {
                Rates = new List<string>()
            };

            foreach (var item in rates.Rates)
            {
                ratesDTO.Rates.Add(
                    $"{CurrencyCode.Currencies.CZK}/{item.currencyCode}={item.amount}");
            }

            return ratesDTO;
        }
    }
}
