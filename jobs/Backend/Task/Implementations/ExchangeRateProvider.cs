using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Service;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace ExchangeRateUpdater.Implementations
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        public ExchangeRateProvider(IScrapper scrapper, IOptions<BankConfig> bankConfig)
        {
            this._scrapper = scrapper;
            this._bank = bankConfig;
        }

        private readonly IOptions<BankConfig> _bank;
        private readonly IScrapper _scrapper;

        public ExchangeRateProvider(IOptions<BankConfig> bank)
        {
            this._bank = bank;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = this.LoadRates();
            // Ignor currencies not included in source code
            var _selectedRates = rates.Courses.CnbTable.List.Where(a => currencies.Any(m => m.Code == a.Code)).ToList();

            var results = new List<ExchangeRate>();

            currencies.ToList().ForEach((source) =>
            {
                _selectedRates.ForEach(target =>
                {
                    if(source.Code != target.Code && rates.Courses.CnbTable.List.Any(m=>m.Code == source.Code))
                    {
                        var rateSource = 0M;
                        var rateTarget = 0M;

                        if (source.Code == "CZK")
                        {
                            rateTarget = rates.Courses.CnbTable.List.Where(_ => _.Code == target.Code).First().Course;
                            results.Add(new ExchangeRate(new Currency(target.Code), source, rateTarget));
                        }
                        else
                        {
                            rateSource = rates.Courses.CnbTable.List.Where(_ => _.Code == source.Code).First().Course;
                            rateTarget = rates.Courses.CnbTable.List.Where(_ => _.Code == target.Code).First().Course;
                            var targetSum = rates.Courses.CnbTable.List.Where(_ => _.Code == target.Code).First().Sum;
                            var course = ((rateTarget/rateSource) / targetSum);
                            results.Add(new ExchangeRate(new Currency(target.Code),source, Decimal.Round(course, 2)));
                        }
                    }
                });
            });

            return results;

            //return rates.Courses.CnbTable.List.Where(a => currencies.Any(m => m.Code == a.Code)).ToList().Select(b =>
            //{
            //    return new ExchangeRate(new Currency(b.Code),new Currency(b.Code), decimal.Parse(b.Course));
            //});
        }

        private BankModel LoadRates()
        {
            var rates = string.Empty;

            Task.Run(async() => rates = await this._scrapper.GetData(this._bank.Value.Banks["CNB"].Path)).Wait();

            return TransformXmlToJson(rates);
        }

        private BankModel TransformXmlToJson(string SerializedXml)
        {
            #region ČNB model
            var doc = new XmlDocument();
            doc.LoadXml(SerializedXml);
            #endregion

            string sJ = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            return JsonConvert.DeserializeObject<BankModel>(sJ);
        }
    }
}
