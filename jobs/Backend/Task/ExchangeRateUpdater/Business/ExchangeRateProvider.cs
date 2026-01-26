using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Business.Processors;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Business
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateProcessor _exchangeRateProcessor;

        public ExchangeRateProvider(IExchangeRateProcessor exchangeRateProcessor)
        {
            _exchangeRateProcessor = exchangeRateProcessor; 
        }
        
        public async Task<Result<IEnumerable<ExchangeRate>>> GetExchangeRates(string[] currencies)
        {
            return await _exchangeRateProcessor.ProcessExchangeRates(currencies);
        }
    }
}
