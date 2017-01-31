using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateProvider.Infrastructure.ApiProxy;
using ExchangeRateProvider.Model;
using Newtonsoft.Json;

namespace ExchangeRateProvider
{
    /// <summary>
    /// Abstract ExchangeRateProvider
    /// </summary>
    public abstract class AbstractExchangeRateProvider: ApiProxy, IExchangeRateProvider
    {
        public AbstractExchangeRateProvider(string controllerName) : base(controllerName)
        {
        }

        public abstract IEnumerable<ExchangeRateDto> GetExchangeRates(IEnumerable<CurrencyDto> currencies);
    }
}
