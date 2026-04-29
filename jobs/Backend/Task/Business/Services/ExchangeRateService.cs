using ExchangeRateUpdater.Business.Models;
using ExchangeRateUpdater.Business.Parameters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient __Client;
        private readonly IExchangeRateParameters __ExchangeRateParameters;
        private readonly ILogger<ExchangeRateService> __Logger;

        // Using dependency injection to better manage all dependencies and testing
        public ExchangeRateService()
            : this(new ExchangeRateParameters(), new HttpClient(), new LoggerFactory().CreateLogger<ExchangeRateService>())
        { }

        internal ExchangeRateService(IExchangeRateParameters exchangeRateParameters, HttpClient httpClient, ILogger<ExchangeRateService> logger)
        {
            // Business logic classes can all extend a base class that logs different types of exception, for example this ArgumentNullException
            __ExchangeRateParameters = exchangeRateParameters ?? throw new ArgumentNullException(nameof(exchangeRateParameters));
            __Client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            __Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        async Task<List<ExchangeRateModel>> IExchangeRateService.GetExchangeRatesAsync()
        {
            List<ExchangeRateModel> _ExchangeRates = new();
            try
            {
                string _DailyRatesEndpoint = $"{__ExchangeRateParameters.BaseURL}{__ExchangeRateParameters.ExchangeRatesDailyEndpoint}?date={__ExchangeRateParameters.Date:yyyy-MM-dd}&lang={__ExchangeRateParameters.Language}";
                ExchangeRateResponseModel _Response = await __Client.GetFromJsonAsync<ExchangeRateResponseModel>(new Uri(_DailyRatesEndpoint));
                _ExchangeRates = _Response.ExchangeRates;
            }
            catch (Exception _Exception)
            {
                __Logger.LogError(_Exception, _Exception.Message);
            }

            return _ExchangeRates;
        }
    }
}
