using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public abstract class ExchangeRateProviderCSV : IExchangeRateProvider
    {
        private Dictionary<int, string> header = new Dictionary<int, string>();
        public string[] Header { get { return header.Values.ToArray(); } }
        public abstract IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
        public abstract char Separator { get; }  // or via config        

        protected string downloadRawData(string apiUrl)
        {
            using(var wc = new WebClient())
            {
                wc.Encoding = new UTF8Encoding();
                return wc.DownloadString(apiUrl);
            }
        }

        protected IEnumerable<ExchangeRate> processCSVData(IEnumerable<string> items, IEnumerable<Currency> currencies = null)
        {
            if (header.Count == 0)
                throw new Exception("CSV file header is not initialized");
            var list = new List<ExchangeRate>();
            foreach (var item in items)
            {
                var er = processCSVLine(item);
                if (er == null)
                    continue;
                if (currencies != null && currencies.Any(x => x.Code == er.TargetCurrency.Code))  // or equal currency classes via GetHashCode override at Currency class
                    list.Add(er);
            }
            return list;
        }

        protected void initCSVHeader(string line)
        {
            var vals = line.TrimEnd('\r').Split(Separator);
            if (vals.Length == 0)
                throw new Exception("Empty file header");
            initHeader(vals);
        }

        protected void initHeader(string[] header)
        {
            this.header.Clear();
            for (int i = 0; i < header.Length; i++)
                this.header.Add(i, header[i]);
        }
        protected ExchangeRate processCSVLine(string line)
        {
            try
            {
                if (string.IsNullOrEmpty(line))
                    return null;
                var vals = line.TrimEnd('\r').Split(Separator);

                if (vals.Length != header.Count)
                {
                    return null; // invalid line, log option
                }
                var cvals = new Dictionary<string, string>();
                for (int i = 0; i < vals.Length; i++)
                {
                    cvals.Add(header[i], vals[i]);
                }
                return processLineData(cvals);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Invalid line data: " + ex.Message);
                return null;
            }
        }

        protected decimal? getDecimal(string name, Dictionary<string, string> vals)
        {
            var val = getString(name, vals);
            if (string.IsNullOrEmpty(val))
                return null;
            if (decimal.TryParse(val, out decimal d))
                return d;
            return null;
        }

        protected int? getInt(string name, Dictionary<string, string> vals)
        {
            var val = getString(name, vals);
            if (string.IsNullOrEmpty(val))
                return null;
            if (int.TryParse(val, out int i))
                return i;
            return null;
        }

        protected string getString(string name, Dictionary<string, string> vals)
        {
            if (vals.ContainsKey(name))
                return vals[name];
            return null;
        }

        protected abstract ExchangeRate processLineData(Dictionary<string, string> cvals);
    }
}
