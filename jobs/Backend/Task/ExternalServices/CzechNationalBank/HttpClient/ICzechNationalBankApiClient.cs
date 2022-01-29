using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Dtos;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient
{
    public interface ICzechNationalBankApiClient
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime dateTime);
    }
}