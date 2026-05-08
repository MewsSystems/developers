using ExchangeRateUpdater.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Sources
{
    public interface IExchangeRateSource
    {
        Task<IEnumerable<ExchangeRateDTO>> GetDailyExchangeRates(string date = null);
    }
}
