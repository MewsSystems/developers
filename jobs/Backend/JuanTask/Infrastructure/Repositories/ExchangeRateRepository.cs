using Application.Abstractions;
using Domain.Entities;
using Infrastructure.Clients.Cnb;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class ExchangeRateRepository : IExchangeRateRepository
    {

        private readonly ICnbzClient _cnbzClient;

        private readonly ILogger<ExchangeRateRepository> _logger;

        private readonly ExchangeRateRepositoryConfiguration _configuration;

        public ExchangeRateRepository(ICnbzClient cnbzClient,
                                      IOptions<ExchangeRateRepositoryConfiguration> configuration,
                                      ILogger<ExchangeRateRepository> logger)
        {
            _cnbzClient = cnbzClient;
            _configuration = configuration.Value;
            _logger = logger;   
        }

        public async Task<IDictionary<string, ExchangeRate>> GetTodayCZKExchangeRatesDictionaryAsync()
        {

            string exchangeRatesCsv = await _cnbzClient.GetExchangeRateAmountCsvAsync(DateTimeOffset.Now);

            if (string.IsNullOrEmpty(exchangeRatesCsv))
                return new Dictionary<string, ExchangeRate>();

            IEnumerable<string> validLines = ValidLines(exchangeRatesCsv);

            (int indexOfAmount, int indexOfCode, int indexOfRate) indexes = GetIndexes(validLines);

            if (NotValidIndexes(indexes))
                return new Dictionary<string, ExchangeRate>();

            Dictionary<string, ExchangeRate> exchangeRates = new Dictionary<string, ExchangeRate>();

            foreach (string line in ValidLinesWithoutTitle(validLines))
            {

                try
                {

                    ExchangeRate? exchangeRate = CreateExchangeRate(line, indexes);

                    if (NotIsValidExchangeRate(exchangeRate, exchangeRates))
                        continue;

                    exchangeRates.Add(exchangeRate.SourceCurrency.Code, exchangeRate);

                }
                catch (Exception ex) 
                {
                    _logger.LogError(ex, ex.Message);
                }

            }

            return exchangeRates;

        }

        private IEnumerable<string> ValidLines(string exchangeRatesCsv)
        {

            if (string.IsNullOrEmpty(exchangeRatesCsv))
                return Enumerable.Empty<string>();

            IEnumerable<string> lines = exchangeRatesCsv.Split('\n');

            return lines.Where(s => s.Contains('|'));

        }

        private ExchangeRate? CreateExchangeRate(string line, (int indexOfAmount, int indexOfCode, int indexOfRate) indexes)
        {

            if (string.IsNullOrEmpty(line))
                return null;

            IEnumerable<string> lineItems = line.Split("|");

            string sourceCurrency = lineItems.ElementAt(indexes.indexOfCode);
            int amount = int.Parse(lineItems.ElementAt(indexes.indexOfAmount));
            decimal exchangeRate = decimal.Parse(lineItems.ElementAt(indexes.indexOfRate));

            return ExchangeRate.Create(sourceCurrency,
                                       _configuration.TargetCurrency,
                                       amount,
                                       exchangeRate);

        }

        private (int indexOfAmount, int indexOfCode, int indexOfRate) GetIndexes(IEnumerable<string> validLines)
        {

            if (validLines is null || !validLines.Any())
                return (0, 0, 0);

            var titleLine = validLines.FirstOrDefault(v => v.ToLower().Contains(_configuration.RateTitle));

            if (string.IsNullOrEmpty(titleLine))
                return (0, 0, 0);

            var titles = titleLine.Split("|").Select(t => t.ToLower()).ToList();

            return (titles.IndexOf(_configuration.AmountTitle), 
                    titles.IndexOf(_configuration.CodeTitle), 
                    titles.IndexOf(_configuration.RateTitle));

        }

        private IEnumerable<string> ValidLinesWithoutTitle(IEnumerable<string> validLines)
        {
            
            if (validLines is null)
                return Enumerable.Empty<string>();

            return validLines.Where(v => !v.ToLower().Contains(_configuration.RateTitle));

        }

        private bool NotValidIndexes((int indexOfAmount, int indexOfCode, int indexOfRate) indexes)
        {
            return indexes.indexOfAmount == indexes.indexOfCode ||
                   indexes.indexOfAmount == indexes.indexOfRate ||
                   indexes.indexOfCode == indexes.indexOfRate;
        }

        private bool NotIsValidExchangeRate(ExchangeRate exchangeRate, Dictionary<string, ExchangeRate> exchangeRates)
        {
            return exchangeRate == null ||
                   exchangeRates.ContainsKey(exchangeRate.SourceCurrency.Code);
        }

    }
}
