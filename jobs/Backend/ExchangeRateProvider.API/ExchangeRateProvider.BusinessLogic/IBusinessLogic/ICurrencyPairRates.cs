using ExchangeRateProvider.DomainEntities;
using ExchangeRateProvider.DomainEntities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.BusinessLogic.IBusinessLogic
{
    public interface ICurrencyPairRates
    {
        Task<RatesDTO> GetAllAsync(RequestModel requestModel);
    }
}
