using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Provider.Cnb.Dtos
{
    public class CnbResponse
    {
        public List<CnbRate> Rates { get; set; }
    }
}
