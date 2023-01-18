using Business.Abstract;
using Business.Constants;
using Common.AutofacAspects.Validation;
using Common.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using Entities.ValidationRules.FluentValidation;
using ExchangeRateUpdater;

namespace Business.Concrete
{
    public class ExchangeRateProviderManager : IExchangeRateProviderService
    {
        private readonly IExchangeRateProvider _exchangeRateProvider;
        public ExchangeRateProviderManager(IExchangeRateProvider exchangeRateProvider)
        {
            _exchangeRateProvider = exchangeRateProvider;
        }

        [ValidationAspect(typeof(CurrencyListValidator))]
        public IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(CurrencyListRecord currencyListRecord)
        {
            var exchangeRates = _exchangeRateProvider.GetExchangeRates();
            

            var returnExchangeRates = exchangeRates
           .Where(exchangeRate =>
               currencyListRecord.Currencies.Any(currency => string.Equals(exchangeRate.TargetCurrency.Code, currency.Code,
                   StringComparison.OrdinalIgnoreCase)))
           .ToArray();
            if (returnExchangeRates.Count() < 1)
            {
                return new DataResult<IEnumerable<ExchangeRate>>(returnExchangeRates, false, Messages.ExchangeRatesOCanNotBeFound);

            }
            return new DataResult<IEnumerable<ExchangeRate>>(returnExchangeRates, true, Messages.ExchangeRatesObtained);
        }
    }
}
