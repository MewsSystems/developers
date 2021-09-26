using System;
using System.Threading.Tasks;
using ExchangeRateUpdater.CnbProxy.Implementation;
using ExchangeRateUpdater.Utilities.Extensions;
using ExchangeRateUpdater.Utilities.Logging;

namespace ExchangeRateUpdater.CnbProxy.Services
{
    class CnbExchangeRatesService : ICnbExchangeRatesService
    {
        private readonly ICnbHttpClient _cnbHttpClient;
        private readonly ICnbXmlDeserializer _cnbXmlDeserializer;
        private readonly IAppLogger _logger;

        public CnbExchangeRatesService(ICnbHttpClient cnbHttpClient, ICnbXmlDeserializer cnbXmlDeserializer, IAppLogger logger)
        {
            Guard.ArgumentNotNull(nameof(cnbHttpClient), cnbHttpClient);
            Guard.ArgumentNotNull(nameof(cnbXmlDeserializer), cnbXmlDeserializer);

            _cnbXmlDeserializer = cnbXmlDeserializer;
            _cnbHttpClient = cnbHttpClient;
            _logger = logger;
        }

        public async Task<kurzy> GetAsync()
        {
            try
            {
                // TODO: Another source https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=
                // I would create a class with mapping between source header and properties in my class and then some parser.
                // Alternatively, I would use CsvHelper https://github.com/JoshClose/CsvHelper
                // which is fine for files smaller than 200MB - 300MB

                return await _cnbHttpClient
                    .GetXmlExchangeRatesAsync().ContinueWith(x => 
                        _cnbXmlDeserializer.Deserialize<kurzy>(x.Result));
            }
            catch (Exception e)
            {
                _logger.Error($"Exchange rate service error: '{e.Message}'");

                throw;
            }
        }
    }
}
