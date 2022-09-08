using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{

    public static class ExchangeRateCache
    {
        private static Dictionary<string, ExchangeRate> data = new Dictionary<string, ExchangeRate>();

    }
}
