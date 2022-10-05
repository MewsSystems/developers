using Common.Configuration;
using Core.Models;
using Core.Parser;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Logging;

namespace Core.Client.CzechNationalBank
{
    public class CzechNationalBankClient : BaseClient, IClient
    { 
        public CzechNationalBankClient
            (ILogger<CzechNationalBankClient> logger, IConfigurationWrapper configurationWrapper, IHttpWrapper httpWrapper, IResponseParser responseParser) 
            : base(logger, configurationWrapper, httpWrapper, responseParser)
        {

        }

        /// <summary>
        /// Custom client implementation for CzechNationalBank
        /// </summary>
        /// <returns>Parsed exchange rates</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            // pull source from config
            var rateSource = _configurationWrapper.GetConfigValueAsString("CzechNationalBankClient:Source");
            var responseContent = await _httpWrapper.HttpGet(rateSource);
            return _responseParser.ParseResponse(responseContent);
        }
    }
}
