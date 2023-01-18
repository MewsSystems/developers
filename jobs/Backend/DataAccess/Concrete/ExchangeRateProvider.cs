using Common.AutofacAspects.Validation;
using DataAccess.Abstract;
using Entities.Models;
using Entities.Records;
using Entities.ValidationRules.FluentValidation;
using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Concrete
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {      
        private IExchangeRateAccessor _exchangeRateAccessor;
        public ExchangeRateProvider(IExchangeRateAccessor exchangeRateAccessor)
        {
           _exchangeRateAccessor = exchangeRateAccessor;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            var currencyRates = _exchangeRateAccessor.GetExchangeRates();
              
            return currencyRates;
        }
    }
}
