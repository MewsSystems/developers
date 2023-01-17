using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Common.Results;
using ExchangeRateUpdater;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = FluentValidation.ValidationException;

namespace Business.Concrete
{
    public class ExchangeRateProviderManager : IExchangeRateProviderService
    {
        public IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var validator = new ValidationContext<IEnumerable<Currency>>(currencies);
            CurrencyListValidator clValidator= new CurrencyListValidator(); 
           var result= clValidator.Validate(validator);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);
            }
           return new DataResult<IEnumerable<ExchangeRate>>(null,true, Messages.ExchangeRatesObtained);
        }
    }
}
