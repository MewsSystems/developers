using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Exceptions;
using ExchangeRate.Application.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExchangeRate.Application.Services
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public class ExchangeRateProviderService : IExchangeRateProviderService
    {
        private readonly IExchangeRateService _exchangeRateService;
        public ExchangeRateProviderService(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        public async Task<ExchangeRateProviderResultDTO> GetExchangeRatesByDate(DateTime date, CurrencyDTO target)
        {
            try
            {
                var exchangeRates = await _exchangeRateService.GetExchangeRatesByDate(date);
                var rates = exchangeRates.ExchangeRates;

                return MapExchangeRate(rates, target);
            }
            catch (Exception)
            {
                throw;
            }



        }
        public async Task<ExchangeRateProviderResultDTO?> GetExchangeRatesByDate(ExchangeRatesDTO currency)
        {
            if (currency.SourceCurrency!.Code != "CZK")
            {
                return null;
            }

            var exchangeRates = await _exchangeRateService.GetExchangeRatesByDate(currency.Date);

            var rates = exchangeRates.ExchangeRates;

            return MapExchangeRate(rates, currency.TargetCurrency!);

        }

        public async Task<ExchangeRateProviderResultDTO> GetExchangeRates()
        {
            var exchangeRates = await _exchangeRateService.GetDailyExchangeRates();
            var currencies = _exchangeRateService.GetCurrenciesBank(exchangeRates).ToList();
            return MapExchangeRates(exchangeRates, currencies);
        }

        public async Task<ExchangeRateProviderResultDTO> GetExchangeRates(CurrenciesDTO currenciesDTO)
        {
            var exchangeRates = await _exchangeRateService.GetDailyExchangeRates();
            var bankCurrencies = _exchangeRateService.GetCurrenciesBank(exchangeRates).ToList();
            var currencies = currenciesDTO.CurrencyCodes.Where(c => c != null).ToList();

            var missingCurrencies = currencies
                .Where(c => !bankCurrencies.Any(bc => bc.Code == c.Code))
                .Select(c => c.Code)
                .ToList(); ;

            if (missingCurrencies.Any())
            {
                throw new CurrencyNotFoundException(missingCurrencies);
            }

            return MapExchangeRates(exchangeRates, currencies);
        }

        private ExchangeRateProviderResultDTO MapExchangeRates(ExchangeRatesBankDTO? exchangeRates, IEnumerable<CurrencyDTO> currencies)
        {
            if (exchangeRates?.ExchangeRates == null || !exchangeRates.ExchangeRates.Any())
            {
                return new ExchangeRateProviderResultDTO(new Dictionary<string, ExchangeRateProviderDTO>());
            }

            var rates = exchangeRates.ExchangeRates;

            var result = currencies
                .Select(currency =>
                {
                    var pairCode = $"CZK/{currency.Code}";
                    var rate = rates.FirstOrDefault(x => x.Code == currency.Code);

                    return new KeyValuePair<string, ExchangeRateProviderDTO>(
                        pairCode,
                        new ExchangeRateProviderDTO(new CurrencyDTO("CZK"), currency, rate!.Rate, rate.Amount)
                    );
                })
                .ToDictionary(x => x.Key, x => x.Value);

            return new ExchangeRateProviderResultDTO(result);
        }

        private ExchangeRateProviderResultDTO MapExchangeRate(List<ExchangeRateBankDTO> rates, CurrencyDTO target)
        {
            if (rates == null || !rates.Any())
            {
                return new ExchangeRateProviderResultDTO(new Dictionary<string, ExchangeRateProviderDTO>()); // Return an empty dictionary if no rates exist
            }

            Dictionary<string, ExchangeRateProviderDTO> exchangeRateProviderDTOs = rates
                .Where(x => x.Code == target.Code)
                .Select(rate =>
                {
                    var pairCode = $"CZK/{target.Code}";
                    var exchangeRateProvider = new ExchangeRateProviderDTO(new CurrencyDTO("CZK"), target, rate.Rate, rate.Amount);
                    return new KeyValuePair<string, ExchangeRateProviderDTO>(pairCode, exchangeRateProvider);
                })
                .ToDictionary(x => x.Key, x => x.Value);

            return new ExchangeRateProviderResultDTO(exchangeRateProviderDTOs);
        }

    }
}
