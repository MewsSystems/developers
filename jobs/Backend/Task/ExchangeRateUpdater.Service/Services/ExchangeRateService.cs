using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Service.Infrastructure.Interfaces;
using ExchangeRateUpdater.Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly ICnbService _cnbService;

        public ExchangeRateService(ICnbService cnbService)
        {
            _cnbService = cnbService;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            return await _cnbService.GetExchangeRatesByCurrencyAsync(DateTime.Now, currencies );
        }
    }
}
