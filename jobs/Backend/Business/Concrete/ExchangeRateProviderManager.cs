using Business.Abstract;
using Business.Constants;
using Common.Results;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ExchangeRateProviderManager : IExchangeRateProviderService
    {
        public IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
           return new DataResult<IEnumerable<ExchangeRate>>(null,true, Messages.ExchangeRatesObtained);
        }
    }
}
