using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRateProvider.External;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Services
{
    public class CzechNationalBankExchangeRateService : IExchangeRateService
    {
        private readonly IEnumerable<IExchangeRateService> _services;

        public CzechNationalBankExchangeRateService(params IExchangeRateService[] services)
        {
            _services = services;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyExchangeRatesAsync(string targetCurrencyCode)
        {
            var getExchangeRatesTasks = _services.Select(s => s.GetCurrencyExchangeRatesAsync(targetCurrencyCode)).ToList();
            await Task.WhenAll(getExchangeRatesTasks);
            return getExchangeRatesTasks.SelectMany(t => t.Result);
        }
    }

    public class ExchangeRateFixingService : IExchangeRateService
    {
        private readonly ICzechNationalBankExchangeRateClient _exchangeRateClient;
        private readonly IExchangeRatesParser _exchangeRatesParser;

        public ExchangeRateFixingService(
            ICzechNationalBankExchangeRateClient exchangeRateClient,
            IExchangeRatesParser exchangeRatesParser)
        {
            _exchangeRateClient = exchangeRateClient;
            _exchangeRatesParser = exchangeRatesParser;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyExchangeRatesAsync(string targetCurrencyCode)
        {
            var exchangeRateFixing = await _exchangeRateClient.GetExchangeRateFixingAsync();
            return _exchangeRatesParser.ExtractCurrencyExchangeRates(targetCurrencyCode, exchangeRateFixing);
        }
    }

    public class ExchangeRateOtherCurrenciesService : IExchangeRateService
    {
        private readonly ICzechNationalBankExchangeRateClient _exchangeRateClient;
        private readonly IExchangeRatesParser _exchangeRatesParser;

        public ExchangeRateOtherCurrenciesService(
            ICzechNationalBankExchangeRateClient exchangeRateClient,
            IExchangeRatesParser exchangeRatesParser)
        {
            _exchangeRateClient = exchangeRateClient;
            _exchangeRatesParser = exchangeRatesParser;
        }

        public async Task<IEnumerable<ExchangeRate>> GetCurrencyExchangeRatesAsync(string targetCurrencyCode)
        {
            var exchangeRateOtherCurrencies = await _exchangeRateClient.GetFixRatesOfOtherCurrenciesAsync();
            return _exchangeRatesParser.ExtractCurrencyExchangeRates(targetCurrencyCode, exchangeRateOtherCurrencies);
        }
    }
}
