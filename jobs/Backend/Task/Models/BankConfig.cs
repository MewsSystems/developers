using System.Collections.Generic;

namespace ExchangeRateUpdater.Models
{
    public class BankConfig
    {
        public IDictionary<string,BankItem> Banks { get; set; }
    }

    public class BankItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
