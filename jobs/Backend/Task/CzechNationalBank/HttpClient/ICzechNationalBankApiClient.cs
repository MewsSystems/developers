using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.CzechNationalBank.HttpClient.Dtos;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient
{
    public interface ICzechNationalBankApiClient
    {
        Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(DateTime dateTime);
    }
}