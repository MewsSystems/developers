using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateProvider.Infrastructure.ApiProxy;
using ExchangeRateProvider.Model;

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
