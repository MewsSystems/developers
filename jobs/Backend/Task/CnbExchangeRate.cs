using ExchangeRateUpdater.Model;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
namespace ExchangeRateUpdater
{
    public class CnbExchangeRate
    {
        private static readonly string CnbExchangeRates = ConfigurationManager.AppSettings["CnbExchangeRateDataUrl"];
        public static List<CnbExchangeRateData> GetCnbExchangeRates()
        {
            WebClient client = new();
            string content = client.DownloadString(CnbExchangeRates);

            List<CnbExchangeRateData> cnbData = new();
            string[] cnbWebDataRows = content.Split('\n');
            if(cnbWebDataRows.Length > 1)
            {
                for(int i = 2; i < cnbWebDataRows.Length; i++)
                {
                    string[] cnbWebData = cnbWebDataRows[i].Split('|');
                    if (cnbWebData.Length >= 4 )
                    {
                        CnbExchangeRateData exchangeRateData = new() { Country = cnbWebData[0], Currency = cnbWebData[1], Amount = cnbWebData[2], Code = cnbWebData[3], Rate = cnbWebData[4] };
                        cnbData.Add(exchangeRateData);
                    }
                }
            }
            return cnbData;
        }
    }
}
