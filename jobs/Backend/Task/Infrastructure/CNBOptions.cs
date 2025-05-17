using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    internal record CNBOptions
    {
        public string BaseUrl { get; set; }
    }
}
