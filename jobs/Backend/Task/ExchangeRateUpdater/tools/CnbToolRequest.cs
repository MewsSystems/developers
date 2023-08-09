using System;
using System.Collections.Specialized;
using System.Net.Http;
using ExchangeRateUpdater.tools;

namespace services
{
    public class CnbToolRequest : IToolRequest
    {
        private const string English = "EN";
        public CnbToolRequest(HttpMethod method, string path, NameValueCollection query = null,object content=null):base(method,path,query,content)
        {
            this.BaseUrlKey = "https://api.cnb.cz/cnbapi";

        }     

        public static CnbToolRequest DailyRatesRequest (DateTime date)
        {
            var path = $"/exrates/daily";
            var query = new NameValueCollection{
                {"date",date.ToString("yyyy-MM-dd")},
                {"lang",English},
            };
            return new CnbToolRequest(HttpMethod.Get, path, query);
        }
        
        public static CnbToolRequest OthersRatesRequest (DateTime date)
        {
            var path = $"/fxrates/daily-month";
            var query = new NameValueCollection{
                {"yearMonth",date.ToString("yyyy-MM")},
                {"lang",English},
            };
            return new CnbToolRequest(HttpMethod.Get, path, query);
        }

    }
}