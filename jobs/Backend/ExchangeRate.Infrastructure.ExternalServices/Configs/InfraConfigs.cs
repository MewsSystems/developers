using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Infrastructure.ExternalServices.Configs
{
    public class InfraConfigs : IInfraConfigs
    {
        public CzechNationalBanckConfigs CzechNatBank {  get; set; } = new CzechNationalBanckConfigs();
    }
}
