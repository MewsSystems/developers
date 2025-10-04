using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Decorator
{
    internal class LoadData : ILoadRates
    {
        readonly Dictionary<string, Rate> rates;

        public LoadData() => rates = new();

        public async Task<bool> Load(string data)
        {
            var lines = new StringReader(data)
                .ReadToEnd()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(2);

            foreach (string line in lines)
            {
                string[] list = line.Split('|');
                rates.Add(list[3], new Rate(list[0], list[1], Convert.ToInt16(list[2]), list[3], Convert.ToDecimal(list[4])));
            }

            return true;
        }
    }
}
