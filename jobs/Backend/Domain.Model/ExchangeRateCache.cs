using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{

    public static class ExchangeRateCache
    {
        public static Dictionary<string,List<ExchangeRate>> Data = new Dictionary<string, List<ExchangeRate>>();


    }
}
