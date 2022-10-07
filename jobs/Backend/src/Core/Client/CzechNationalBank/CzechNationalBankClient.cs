using Common.Configuration;
using Core.Models;
using Core.Parser;
using ExchangeRateUpdater.Client;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static Common.Enums;

namespace Core.Client.CzechNationalBank
{
    /// <summary>
    /// Client implementation for CzechNationalBank
    /// </summary>
    public class CzechNationalBankClient : BaseClient, IClient
    { 
        public CzechNationalBankClient
            (ILogger<CzechNationalBankClient> logger, IConfigurationWrapper configurationWrapper, IHttpWrapper httpWrapper, IResponseParser responseParser) 
            : base(logger, configurationWrapper, httpWrapper, responseParser)
        {

        }

        /// <summary>
        /// GetExchange rates implementation for CzechNationalBank
        /// </summary>
        /// <returns>Parsed exchange rates</returns>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            var command = "CzechNationalBank.GetExchangeRates";
            var request = "";
            string responseContent = "";
            var status = ResponseStatus.FAILURE;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                // pull source from config
                request = _configurationWrapper.GetConfigValueAsString("CzechNationalBankClient:Source");
                responseContent = await _httpWrapper.HttpGet(request);
                status = ResponseStatus.SUCCESS;
                // parse response
                return _responseParser.ParseResponse(responseContent);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                stopwatch.Stop();
                // log outgoing call statistics 
                LogOutGoingCall(request, responseContent, command, status, stopwatch.Elapsed);
            }

            return Enumerable.Empty<ExchangeRate>();
        }
    }
}
