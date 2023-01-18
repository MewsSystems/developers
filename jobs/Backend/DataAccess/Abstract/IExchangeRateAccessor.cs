using Entities.Models;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IExchangeRateAccessor
    {
        IEnumerable<ExchangeRate> GetExchangeRates();
    }
}
