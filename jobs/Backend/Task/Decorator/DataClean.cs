using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Decorator
{
    internal class DataClean : LoadRates
    {
        private string _line;
        private StringBuilder _correctLines;

        public DataClean(ILoadRates wrapper) : base(wrapper)
        {
            _correctLines = new();
        }

        public override Task<bool> Load(string data)
        {
            using (StringReader reader = new(data))
            {
                while ((_line = reader.ReadLine()) != null)
                {
                    if (Regex.IsMatch(_line, @"^[A-Za-z ]+\|[A-Za-z ]+\|(1|100|1000)\|[A-Z]{3}\|\d+\.\d+$"))
                    {
                        _correctLines.AppendLine(_line);
                    }
                }
            }

            return wrapper.Load(_correctLines.ToString());
        }
    }
}
