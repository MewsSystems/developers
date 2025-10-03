using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.CNB;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Decorator
{
    internal class ReadResult
    {
        //readonly List<Rate> rates;
        readonly Dictionary<string, Rate> rates;

        public ReadResult()
        {
            rates = new();
        }

        public async Task<Dictionary<string, Rate>> ReadString() //string
        {
            APICall aPICall = new();

            string result = await aPICall.DayliExchange();

            var lines = new StringReader(result)
                .ReadToEnd()
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Skip(2);

            foreach(string line in lines)
            {
                string[] list = line.Split('|');
                //rates.Add(new Rate(list[0], list[1], Convert.ToInt16(list[2]), list[3], Convert.ToDouble(list[4])));
                rates.Add(list[3], new Rate(list[0], list[1], Convert.ToInt16(list[2]), list[3], Convert.ToDecimal(list[4])));
            }

            return rates;
        }
    }
}