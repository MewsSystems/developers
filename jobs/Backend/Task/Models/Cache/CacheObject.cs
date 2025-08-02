using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models.Cache
{
    public class CacheObject<T>
    {
        public T Data { get; set; }
        public DateTimeOffset DataExtractionTimeUTC { get; set; }
    }
}
