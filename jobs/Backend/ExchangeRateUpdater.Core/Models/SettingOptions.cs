using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Models
{
    public class SettingOptions
    {
        public List<string> AllowedCurrencies { get; set; } = new();
        public string Endpoint { get; set; } = string.Empty;
        public string BaseUrl { get; set; } = string.Empty;
    }
}
