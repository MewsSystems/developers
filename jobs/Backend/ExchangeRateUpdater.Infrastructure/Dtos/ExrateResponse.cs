using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure.Dtos
{
    public class ExrateResponse
    {
        public List<ExrateDto> Rates { get; set; } = new();
    }
}
