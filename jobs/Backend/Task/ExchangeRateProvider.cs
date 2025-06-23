using AutoMapper;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.Service.Services.Interface;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IMapper _mapper;

        public ExchangeRateProvider(IExchangeRateService exchangeRateService, IMapper mapper)
        {
            _exchangeRateService = exchangeRateService;
            _mapper = mapper;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRateResponseDto> GetExchangeRates(IEnumerable<CurrencyRequestDto> currencies)
        {

            var exchangeRates = _exchangeRateService.GetExchangeRatesAsync(_mapper.Map<IEnumerable<Currency>>(currencies)).Result;

            return _mapper.Map<IEnumerable<ExchangeRateResponseDto>>(exchangeRates);
        }
    }
}
