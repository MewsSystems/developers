using System;
using System.IO;
using System.Net;
using System.Text;
using ExchangeRateUpdater.ServiceContracts;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Implementation of <see cref="IExchangeRateService"/>.
    /// </summary>
    public class ExchangeRateService : IExchangeRateService
    {
        private const string CacheFolder = "ExchangeRates";
        private const string ExchangeRateUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=";
        private const string ExchangeDateFormat = "dd.MM.yyyy"; // acceptable by CNB

        /// <inheritdoc />
        public LoadExchangeRatesResponse LoadExchangeRates(LoadExchangeRatesRequest loadCurrenciesRequest)
        {
            string exchangeDate = !string.IsNullOrEmpty(loadCurrenciesRequest.ExchangeDate)
                ? loadCurrenciesRequest.ExchangeDate
                : DateTime.Now.ToString(ExchangeDateFormat);

            string cacheFileName = $"{exchangeDate}.txt";
            string cacheFileFullPath = Path.Combine(CacheFolder, cacheFileName);

            if (loadCurrenciesRequest.UseCache)
            {
                if (File.Exists(cacheFileFullPath))
                {
                    try
                    {
                        string[] lines = File.ReadAllLines(cacheFileFullPath);
                        return new LoadExchangeRatesResponse()
                        {
                            Lines = lines
                        };
                    }
                    catch (Exception)
                    {
                        // Load data using WebClient in the next block
                    }
                }
            }

            // if not loaded from cache
            string date = !string.IsNullOrEmpty(loadCurrenciesRequest.ExchangeDate)
                ? loadCurrenciesRequest.ExchangeDate
                : DateTime.Now.ToString(ExchangeDateFormat);

            string url = ExchangeRateUrl + date;

            using (var rawDataMemoryStream = new MemoryStream())
            {
                using (var webClient = new WebClient())
                {
                    using (var rawStream = webClient.OpenRead(url))
                    {
                        // Possible null ref exception
                        if (rawStream == null)
                        {
                            throw new WebException($"Unable to open a web client stream at url {url}",
                                WebExceptionStatus.UnknownError);
                        }

                        rawStream.CopyTo(rawDataMemoryStream);
                    }
                }

                var resultArray = rawDataMemoryStream.ToArray();

                // do other stuff after closing connection
                if (loadCurrenciesRequest.UseCache)
                {
                    try
                    {
                        if (!Directory.Exists(CacheFolder))
                        {
                            Directory.CreateDirectory(CacheFolder);
                        }

                        File.WriteAllBytes(cacheFileFullPath, resultArray);
                    }
                    catch (Exception)
                    {
                        // IO errors should not prevent the response returning
                    }
                }

                string[] lines = Encoding.UTF8.GetString(resultArray).Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                return new LoadExchangeRatesResponse()
                {
                    Lines = lines
                };
            }
        }
    }
}
