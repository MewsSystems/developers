using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public class CurrencyParser : ICurrencyParser
    {        
        public List<CurrencyDTO> Parse(List<ExchangeRateBankDTO> exchangeRates)
        {
            return exchangeRates.Select(x => new CurrencyDTO(x.Code)).Distinct().ToList();
        }
    }
}