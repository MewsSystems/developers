using Common.Results;
using Entities.Dtos;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IExchangeRateProviderService
    {
        IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(ConcurrencListRecord currencies);
    }
}
