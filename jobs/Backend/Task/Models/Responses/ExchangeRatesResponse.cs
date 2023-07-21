using ExchangeRateUpdater.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Services.Responses
{
    public class ExchangeRatesResponse
    {
        public List<Rate> Rates { get; set; }
    }
}
