using ExchangeRateUpdater.AzFunction.Logic.ExchangeRateProvider;
using ExchangeRateUpdater.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExchangeRateUpdater.AzFunction
{
    public class ExchangeRateUpdaterFunction
    {
        private readonly IExchangeRateProviderManager _exchangeRateProviderManager;
        public ExchangeRateUpdaterFunction(IExchangeRateProviderManager exchangeRateProviderManager)
        {
            _exchangeRateProviderManager = exchangeRateProviderManager;
        }

        /// <summary>
        /// We talked about Service Bus, Logic Apps, SOA architectures, Azure... So I decided to create an API using Azure Functions. 
        /// I like to keep it simple. In Azure time is money. We can talk about caching the results.
        /// Anyway, in a SOA architecture using Azure, Azure API Management is really useful. It provides a caching system, security.. all the stuff
        /// required in order to don't reinvent the wheel.
        /// </summary>
        /// <param name="req">Request with the currencies to obtains</param>
        /// <param name="log">Default log</param>
        /// <returns></returns>
        [FunctionName(nameof(ExchangeRateUpdaterFunction))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            var currency = req.Headers["TargetCurrency"];
            if (string.IsNullOrWhiteSpace(currency))
                return new BadRequestResult();

            try
            {
                log.LogInformation($"Function {nameof(ExchangeRateUpdaterFunction)} started.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var currencies = JsonConvert.DeserializeObject<List<Currency>>(requestBody);


                IExchangeRateProvider exchangeProvider = _exchangeRateProviderManager.GetExchangeRateProvider(currency);
                var exchange = await exchangeProvider.GetExchangeRates(currencies);

                log.LogInformation($"Function {nameof(ExchangeRateUpdaterFunction)} ended.");

                return new OkObjectResult(exchange);
            }
            catch (Exception)
            {
                return new InternalServerErrorResult();
            }
        }
    }
}
