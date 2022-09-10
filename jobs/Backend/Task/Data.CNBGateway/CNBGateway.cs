using CNBClient.Interfaces;
using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.CNBGateway
{
   
    public class CNBGateway : ICNBGateway
    {
        private readonly ICNBExchageRateUpdater _updaterClient;


        public CNBGateway(ICNBExchageRateUpdater updaterClient)
        {
            _updaterClient = updaterClient;
        }

        public string LoadDataFromServer() => _updaterClient.LoadFromServer();
        
        

    }
}
