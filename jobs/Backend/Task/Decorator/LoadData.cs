using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Singleton;

namespace ExchangeRateUpdater.Decorator
{
    public class LoadData : ILoadRates
    {
        private string _line;
        private DB _rates;

        public LoadData() => _rates = DB.GetInstance();

        public async Task<bool> Load(string data)
        {
            try
            {
                using (StringReader reader = new(data))
                {
                    while ((_line = reader.ReadLine()) != null)
                    {
                        string[] list = _line.Split('|');
                        _rates.Add(list[3], new Rate(list[0], list[1], Convert.ToInt16(list[2]), list[3], Convert.ToDecimal(list[4], CultureInfo.InvariantCulture)));
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (FormatException)
            {
                throw;
            }

            return !_rates.IsEmpty();
        }
    }
}
