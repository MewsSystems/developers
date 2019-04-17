using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Diagnostics;
using ExchangeRateUpdater.PZ.Models;

namespace ExchangeRateUpdater.PZ.Http
{
    public class CnbClient
    {
        public delegate void CnbClientException(Exception exception);

        protected CnbClient()
        { }
        public CnbClient(string link = @"http://www.cnb.cz/")
        {
            Link = link;
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(Link)
        };
        }
        #region Properties
        public string Link { get; set; }
        public HttpClient HttpClient { get; }
        public CnbClientException clientException { get; set; }
        #endregion
        public async Task<IEnumerable<MyCurrency>> ExchangeDataAsync()
        {
            List<MyCurrency> myCurrencies = new List<MyCurrency>();
            List<string> temp = await GetNetwork();
            foreach(string row in temp)
            {
                myCurrencies.Add(MyCurrency.GetMyCurrency(row));
            }
            return myCurrencies;
        }

        async Task<List<string>> GetNetwork()
        {
            List<string> temp = new List<string>();

            using (HttpClient)
            {
                try
                {
                    HttpResponseMessage response = await HttpClient.GetAsync(FindMeLinkToSource());
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    temp = responseBody.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    temp.RemoveRange(0,2);
                }
                catch (Exception e)
                {
                    clientException?.Invoke(e);
                }

                return temp;
            }
        }

        private string FindMeLinkToSource()
        {
            string webPage;
            using (HttpResponseMessage response = HttpClient.GetAsync(Link).Result)
            using (HttpContent content = response.Content)
            {
                webPage = content.ReadAsStringAsync().Result;
            }
            var rows = webPage.Split('\n').ToList();
            string allRates = rows.Single(x => x.ToLower().Contains("courses__all"));
            var s = allRates.Split(' ').ToList();
            var ToRates = s.Single(x => x.ToLower().Contains("href"));
            var linkToRates = ToRates.Split('"');

            string finallink = $"{Link}{linkToRates[1]}denni_kurz.txt";

            return finallink;
        }
    }
}
