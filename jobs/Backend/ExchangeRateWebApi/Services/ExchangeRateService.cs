using ExchangeRateUpdater;
using ExchangeRateUpdater.Models;
using ExchangeRateWebApi.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text;

namespace ExchangeRateWebApi.Services
{
    public class ExchangeRateService : IExchangeRate
    {
        private readonly AddressOptions options;
        private readonly ILogger<ExchangeRateService> logger;

        public ExchangeRateService(IOptions<AddressOptions> options, ILogger<ExchangeRateService> logger)
        {
            this.options = options.Value;
            this.logger = logger;
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("cs-CZ");
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
        public List<DataNode> GetData(string url)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                var htmlDoc = web.Load(url);
                var first = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"apollo-page\"]/section/div[2]/div/div/main/div/div/div[1]/div[3]/table/tbody").First();
                var dataNodes = first.ChildNodes.Where(x => x.Name == "tr").ToList();
                return dataNodes.Select(x => new DataNode(x)).ToList();

            }
            catch (Exception ex)
            {

                logger.LogError("Error at GetData", ex);
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<ExchangeRate> MapDataToExchangeRates()
        {
            var data = GetData(options.Url);
            List<ExchangeRate> exchangeRates = data.Select(x => new ExchangeRate(new Currency("CZK"), new Currency(x.IsoCode), x.ExchangeRate)).ToList();
            return exchangeRates;
        }
    }
}



