using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    class FileParser
    {
        public List<ExchangeRate> GetDataFromServer(DataSourceEnum dataSource)
        {
            List<ExchangeRate> eRates = new List<ExchangeRate>();
            switch (dataSource)
            {
                case DataSourceEnum.CzechNationalBank:
                    return GetRatesFromCzechNationalBank();
                case DataSourceEnum.EuropeCentralBank:
                    return GetRatesFromEuropeCentralBank();
                case DataSourceEnum.CustomsAdministrationXML:
                    return GetRatesFromCustomsAdministration(true);
                case DataSourceEnum.CustomsAdministrationTXT:
                    return GetRatesFromCustomsAdministration();
                default:
                    throw new ArgumentException("Not implemented datasource parser");
            }
        }

        /// <summary>
        /// Returning data from xml or txt depend on bool
        /// </summary>
        /// <param name="fileType">If true return data from xml. Default return data from txt.</param>
        private List<ExchangeRate> GetRatesFromCustomsAdministration(bool fileType = false)
        {
            var ca = fileType ? DataSourceRefeces.Sources[DataSourceEnum.CustomsAdministrationXML] : DataSourceRefeces.Sources[DataSourceEnum.CustomsAdministrationTXT];
            string file;
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                file = webClient.DownloadString(ca.WebSouce);
            }

            List<ExchangeRate> rates = new List<ExchangeRate>();

            if (fileType == false)
                rates = ParseFromTXT(file, ca, new Currency("CZK"));
            else
            {
                XDocument doc = XDocument.Parse(file);
                var tarCur = new Currency("CZK");
                var elements = doc.Root.Descendants().Where(el => el.Name.LocalName == "kurz");

                foreach (var elem in elements)
                {
                    var innerElem = elem.Descendants();
                    var sourceCur = new Currency(innerElem.Where(el=>el.Name.LocalName==ca.NameCode).First().Value.ToUpper());

                    decimal amount = 0;
                    decimal.TryParse(innerElem.Where(el => el.Name.LocalName == ca.NameAmout).First().Value.ToString(), out amount);

                    decimal rate = 0;
                    decimal.TryParse(innerElem.Where(el => el.Name.LocalName == ca.NameRate).First().Value.ToString(), out rate);

                    rate = amount > 1 ? rate / amount : rate;

                    rates.Add(new ExchangeRate(sourceCur, tarCur, rate));
                }

            }
            return rates;
        }

        private List<ExchangeRate> GetRatesFromEuropeCentralBank()
        {
            var ecb = DataSourceRefeces.Sources[DataSourceEnum.EuropeCentralBank];
            string file;
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                file = webClient.DownloadString(ecb.WebSouce);
            }

            XDocument doc = XDocument.Parse(file);
            List<ExchangeRate> rates = new List<ExchangeRate>();
            var tarCur = new Currency("EUR");
            var elements = doc.Descendants().Where(el => el.Name.LocalName == "Cube" && el.Attribute(ecb.NameCode) != null);

            foreach (var elem in elements)
            {
                var sourceCur = new Currency(elem.Attribute(ecb.NameCode).Value.ToUpper());
                decimal rate = 0;
                decimal.TryParse(elem.Attribute(ecb.NameRate).Value.ToString(),NumberStyles.Any,CultureInfo.InvariantCulture, out rate);


                rates.Add(new ExchangeRate(sourceCur, tarCur, rate));
            }

            return rates;

        }

        private List<ExchangeRate> GetRatesFromCzechNationalBank()
        {
            var cnb = DataSourceRefeces.Sources[DataSourceEnum.CzechNationalBank];
            string file;
            using (var webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                file = webClient.DownloadString(cnb.WebSouce);
            }
            List<ExchangeRate> rates = ParseFromTXT(file, cnb, new Currency("CZK"));
            return rates;
        }

        private List<ExchangeRate> ParseFromTXT(string file,KeyFileProperties propFile, Currency targetCurr)
        {
            List<ExchangeRate> rates = new List<ExchangeRate>();
            string[] lines = file.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            int numCode = 0, numAmount = 0, numRate = 0;
            foreach (var line in lines)
            {
                var lineDetail = line.Split(propFile.Separator);
                if (lineDetail.Length < 3)
                    continue;

                if (lineDetail.Any(cell => cell == propFile.NameCode))
                {
                    numCode = lineDetail.Select((p, i) => new { Item = p, Index = i }).Where(p => p.Item == propFile.NameCode).Select(p => p.Index).First();
                    numAmount = lineDetail.Select((p, i) => new { Item = p, Index = i }).Where(p => p.Item == propFile.NameAmout).Select(p => p.Index).First();
                    numRate = lineDetail.Select((p, i) => new { Item = p, Index = i }).Where(p => p.Item == propFile.NameRate).Select(p => p.Index).First();
                    continue;
                }

                if ((numCode > 0)||(numAmount > 0)|| (numRate > 0))
                {

                    var sourceCur = new Currency(lineDetail[numCode].ToUpper());
                    decimal amount = 0;
                    decimal.TryParse(lineDetail[numAmount], out amount);

                    decimal rate = 0;
                    decimal.TryParse(lineDetail[numRate], out rate);

                    rate = amount > 1 ? rate / amount : rate;

                    rates.Add(new ExchangeRate(sourceCur, targetCurr, rate));
                }
                else
                {
                    throw new FormatException("File is not expected format");
                }

            }
            return rates;
        }
    }
}
