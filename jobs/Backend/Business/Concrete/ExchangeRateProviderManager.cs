using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Common.CrossCuttingConcerns.Validation;
using Common.Results;
using ExchangeRateUpdater;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ExchangeRateProviderManager : IExchangeRateProviderService
    {
        public IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            CommonValidationTool.Validate(new CurrencyListValidator(),currencies);
           return new DataResult<IEnumerable<ExchangeRate>>(null,true, Messages.ExchangeRatesObtained);
        }
    }
}
