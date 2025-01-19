using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Domain.DTO;
using ExchangeRateUpdater.Domain.Models;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateService
    {
        public Task<ExchangeRatesDTO> GetExchangeRateAsync();
    }
}
