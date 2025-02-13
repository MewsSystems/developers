using ExchangeRate.Infrastructure.ExternalServices.CzechNationalBank.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.Builders
{
    public interface IBuildExchangeRates
    {
        List<ExchangeRateBank> BuildExchangeRates(string fileTxt);
    }
}
