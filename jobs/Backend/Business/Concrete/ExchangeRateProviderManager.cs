using Business.Abstract;
using Business.Constants;
using Common.AutofacAspects.Validation;
using Common.Results;
using DataAccess.Abstract;
using Entities.Dtos;
using Entities.ValidationRules.FluentValidation;
using Entities.Concrete;

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
            var exchangeRates = _exchangeRateProvider.GetExchangeRates(currencyListRecord.Currencies);
            
            if (exchangeRates.Count() < 1)
            {
                return new DataResult<IEnumerable<ExchangeRate>>(exchangeRates, false, Messages.ExchangeRatesOCanNotBeFound);
            }
            return new DataResult<IEnumerable<ExchangeRate>>(exchangeRates, true, Messages.ExchangeRatesObtained);
        }
    }
}
