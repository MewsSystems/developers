using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Service
{
    public interface IExchangeRateService
    {
        Task<ExchangeRateResponse> GetDailyExchangeRates(CancellationToken cancellationToken);
    }
}