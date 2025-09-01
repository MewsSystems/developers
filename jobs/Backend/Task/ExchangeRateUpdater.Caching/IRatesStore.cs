using ExchangeRateUpdater.Provider.Cnb.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Caching
{
    public interface IRatesStore
    {
        CnbResponse? Get();
        void SetIfNewer(CnbResponse candidate);
    }
}
