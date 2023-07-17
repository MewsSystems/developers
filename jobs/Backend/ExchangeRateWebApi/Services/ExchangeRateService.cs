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
        public async Task<List<DataNode>> GetDataAsync(string url)
        {
            try
            {
                string content = string.Empty;
                using (var client = new HttpClient())
                {
                    var date = await client.GetAsync(url);
                    content = await date.Content.ReadAsStringAsync();
                }
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(content);
                var first = htmlDoc.DocumentNode.SelectNodes("//div/table/tbody").First();
                var dataNodes = first.ChildNodes.Where(x => x.Name == "tr").ToList();
                return dataNodes.Select(x => new DataNode(x)).ToList();
            }
            catch (Exception ex)
            {

                logger.LogError("Error at GetData", ex);
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ExchangeRate>> MapDataToExchangeRatesAsync()
        {
            var data = await GetDataAsync(options.Url);
            List<ExchangeRate> exchangeRates = data.Select(x => new ExchangeRate(new Currency("CZK"), new Currency(x.IsoCode), x.ExchangeRate)).ToList();
            return exchangeRates;
        }

    }
}



