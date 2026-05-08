using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.CrossCutting.Configurations
{
    public interface IInfraConfigs
    {
        public CzechNationalBanckConfigs CzechNatBank { get; set; }

        // Any other infra configuration goes here:
    }
}
