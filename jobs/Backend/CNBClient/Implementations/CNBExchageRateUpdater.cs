using CNBClient.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNBClient.Implementations
{
    public class CNBExchageRateUpdater : ICNBExchageRateUpdater
    {
        private static readonly string DAILY_EXCHANGE_RATE_URL = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";


        public string LoadFromServer()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (HttpClient client = new HttpClient())
                {
                    using (var response = client.GetAsync(DAILY_EXCHANGE_RATE_URL, HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            using (var stream = response.Content.ReadAsStreamAsync().Result)
                            {
                                using (var sReader = new StreamReader(stream))
                                {
                                    while (!sReader.EndOfStream)
                                    {
                                        string rawData = sReader.ReadLine();

                                        sb.AppendLine(rawData);
                                    }
                                }
                            }
                        }
                    }
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
