using System.Collections.Generic;
using System.Linq;
using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRate.Application.Services
{
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IExchangeRateService _exchangeRateService;
        public ExchangeRateProviderService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<Dictionary<string, ExchangeRateProviderDTO>> GetExchangeRatesByDate(DateTime date, CurrencyDTO currencyDTO)
        {
            var exchangeRates = await _exchangeRateService.GetExchangeRatesByDay(date);
            
            var currencies = exchangeRates.Currencies;
            var rates = exchangeRates.ExchangeRates;

            Dictionary<string, ExchangeRateProviderDTO> exchangeRateProviderDTOs = new();

            foreach ( var currency in currencies)
            {
                if(currency.Code == currencyDTO.Code)
                {
                    var pairCode = $"CZK/{currencyDTO.Code}";
                    exchangeRateProviderDTOs.Add(
                        pairCode,
                        new ExchangeRateProviderDTO(new CurrencyDTO("CZK"), currencyDTO, rates.Where(x => x.Code == currencyDTO.ToString()).First().Rate));
                }
              
            }
            return exchangeRateProviderDTOs;
         
        }

        public async Task<Dictionary<string, ExchangeRateProviderDTO>> GetExchangeRates()
        {
            var exchangeRates = await _exchangeRateService.GetDailyExchangeRates();

            var currencies = exchangeRates.Currencies;
            var rates = exchangeRates.ExchangeRates;

            Dictionary<string, ExchangeRateProviderDTO> exchangeRateProviderDTOs = new();

            foreach (var currency in currencies)
            {
                var pairCode = $"CZK/{currency}";
                exchangeRateProviderDTOs.Add(
                    pairCode,
                    new ExchangeRateProviderDTO(new CurrencyDTO("CZK"), currency, rates.Where(x => x.Code == currency.ToString()).First().Rate));
            }
            return exchangeRateProviderDTOs;
        }

    }
}
