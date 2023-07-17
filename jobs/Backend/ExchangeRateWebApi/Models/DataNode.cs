using HtmlAgilityPack;

namespace ExchangeRateWebApi.Models
{
    public class DataNode
    {
        public string CountryName { get; set; }
        public string CurrencyName { get; set; }
        public int OneCrown { get; set; }
        public string IsoCode { get; set; }
        public decimal ExchangeRate { get; set; }

        public DataNode(HtmlNode trParent)
        {
            var childNodes = trParent.ChildNodes.Where(x => x.Name == "td").ToList();
            if (childNodes.Count != 5)
            {

                throw new Exception("Data in wrong format");
            }

            CountryName = childNodes[0].InnerText;
            CurrencyName = childNodes[1].InnerText;
            OneCrown = int.Parse(childNodes[2].InnerText);
            IsoCode = childNodes[3].InnerText;
            ExchangeRate = decimal.Parse(childNodes[4].InnerText);
        }
    }
}
