using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNBClient.Interfaces
{
    public interface ICNBExchageRateUpdater
    {
        string LoadFromServer();
    }
}
