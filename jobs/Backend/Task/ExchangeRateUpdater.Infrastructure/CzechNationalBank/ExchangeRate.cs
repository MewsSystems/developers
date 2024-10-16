using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.CzechNationalBank
{
    public record ExchangeRate(string CurrencyCode, decimal Rate);
}
