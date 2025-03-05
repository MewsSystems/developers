using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Core.Entities
{
    /// <summary>
    /// Represents a currency using its ISO 4217 code.
    /// </summary>
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public string Code { get; }

        public override string ToString() => Code;
    }
}
