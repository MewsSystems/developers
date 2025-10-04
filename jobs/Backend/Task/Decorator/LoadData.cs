using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater.Decorator
{
    internal class LoadData : ILoadRates
    {
        private string _line;
        private DB rates;

        public LoadData() => rates = DB.GetInstance();

        public async Task<bool> Load(string data)
        {
            using (StringReader reader = new(data))
            {
                while ((_line = reader.ReadLine()) != null)
                {
                    string[] list = _line.Split('|');
                    rates.Add(list[3], new Rate(list[0], list[1], Convert.ToInt16(list[2]), list[3], Convert.ToDecimal(list[4], CultureInfo.InvariantCulture)));
                }
            }

            return true;
        }
    }
}
