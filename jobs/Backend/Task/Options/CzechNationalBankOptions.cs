using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Options
{
    internal class CzechNationalBankOptions
    {
        public static string SectionName { get; } = "CzechNationalBankOptions";

        public string Url { get; set; }    
    }
}
