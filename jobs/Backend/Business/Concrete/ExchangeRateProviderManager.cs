using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Common.AutofacAspects.Validation;
using Common.CrossCuttingConcerns.Validation;
using Common.Results;
using Entities.Dtos;
using Entities.ValidationRules.FluentValidation;
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
        [ValidationAspect(typeof(CurrencyListValidator))]
        public IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(ConcurrencListRecord currencies)
        {
           return new DataResult<IEnumerable<ExchangeRate>>(null,true, Messages.ExchangeRatesObtained);
        }
    }
}
