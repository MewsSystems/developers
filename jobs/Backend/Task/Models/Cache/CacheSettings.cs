using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Models.Cache
{
    public class CacheSettings
    {
        public string Provider { get; set; } = "Memory"; // or "Redis"
        public string RedisConfiguration { get; set; } = string.Empty;
    }
}
