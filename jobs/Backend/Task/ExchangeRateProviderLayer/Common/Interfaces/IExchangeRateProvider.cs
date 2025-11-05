using Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IExchangeRateProvider
    {
        public Task<((int, string), List<ExchangeRateDTO>)> GetExchangeRatesForToday();
        public Task<((int, string), List<ExchangeRateDTO>)> GetHistoryExchangeRates();
    }
}
