using ExchangeRateUpdater.Data.Responses;
using ExchangeRateUpdater.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Business.Interfaces
{
    public interface IExchangeRateService
    {
        Task<List<ExchangeRateResultDto>> GetExchangeRates(IEnumerable<ExchangeRateRequest> currencies, DateTime date, CancellationToken cancellationToken);
    }
}
