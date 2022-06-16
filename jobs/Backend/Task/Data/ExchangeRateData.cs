namespace ExchangeRateUpdater.Data
{
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Domain;
    using Polly.Retry;
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    public class ExchangeRateData : IExchangeRateData
    {
        private readonly Uri uri;
        private readonly ILogger logger;
        private readonly IRetryPolicy<BankDetails> exchangeRateRetryPolicy;

        public ExchangeRateData(Uri uri, ILogger logger, IRetryPolicy<BankDetails> retryPolicy)
        {
            this.uri = uri;
            this.logger = logger;
            exchangeRateRetryPolicy = retryPolicy;
        }

        public BankDetails GetExchangeRateData()
        {
            try
            {
                return exchangeRateRetryPolicy.ExecuteWithRetry(() =>
                    (BankDetails)new XmlSerializer(typeof(BankDetails))
                        .Deserialize(XDocument
                        .Load(uri.ToString())
                        .CreateReader()));
                
            }
            catch(FileNotFoundException fnfe)
            {
                logger.LogError("Please check the URI is correct", fnfe);
                throw;
            }
            catch(HttpRequestException re)
            {
                logger.LogError("Please check the URI is correct", re);
                throw;
            }
            catch (InvalidOperationException ex)
            {
                logger.LogError("There was an invalid operation", ex);
                throw;
            }
        }
    }
}
