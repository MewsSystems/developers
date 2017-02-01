using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProvider.Model
{
    /// <summary>
    /// Currency data transfer
    /// </summary>
    [Serializable]
    public class CurrencyDto
    {
        public string Code { get; set; }
    }

}
