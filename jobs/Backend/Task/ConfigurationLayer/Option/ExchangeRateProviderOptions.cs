using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigurationLayer.Option
{
    public class ExchangeRateProviderOptions
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string BaseCurrency { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool RequiresAuthentication { get; set; } = false;
        public Dictionary<string, string> Configuration { get; set; } = new();
    }

    public class ExchangeRateProvidersOptions
    {
        public ExchangeRateProviderOptions CNB { get; set; } = new();
        public ExchangeRateProviderOptions ECB { get; set; } = new();
        public ExchangeRateProviderOptions BNR { get; set; } = new();
    }
}
