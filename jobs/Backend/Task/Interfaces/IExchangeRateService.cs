using ExchangeRateUpdater.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateService
{
    Task<ExchangeRateListDto> GetExchangeRateListAsync();
}
