using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.IO;
using System.Linq;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        public HttpClient HttpClient { get; private set; }

        [TestMethod]
        public void FindSourceData()
        {
            string Link = @"http://www.cnb.cz";
            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(Link)
            };
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

            string finallink = $"{Link}{linkToRates[1]}denni_kurz.txt";//string.Join(Link, linkToRates[1], "denni_kury.txt");

        }
    }
}
