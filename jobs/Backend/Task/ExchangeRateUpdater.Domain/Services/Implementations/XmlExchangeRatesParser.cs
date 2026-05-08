using System.Xml.Serialization;
using ExchangeRateUpdater.Domain.ApiClients.Models;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Domain.Services.Implementations;
public sealed class XmlExchangeRatesParser(ILogger<XmlExchangeRatesParser> logger) : IExchangeRateParser
{
    public Task<IEnumerable<ExchangeRate>> ParseAsync(string xmlContent)
    {
        var rates = new List<ExchangeRate>();
        try
        {
            var serializer = new XmlSerializer(typeof(ExchangeRatesResponse));
            using var reader = new StringReader(xmlContent);
            var deserializedContent = serializer.Deserialize(reader) as ExchangeRatesResponse;

            if (deserializedContent == null || deserializedContent.Tables.Length <= 0)
            {
                logger.LogWarning("Deserialized ExchangeRates Xml content is null or empty");
                return Task.FromResult<IEnumerable<ExchangeRate>>([]);
            }

            foreach (var table in deserializedContent.Tables)
            {
                foreach (var row in table.Rows)
                {
                    if(string.IsNullOrEmpty(row.CurrencyCode))
                        continue;
                    
                    rates.Add(new ExchangeRate
                    {
                        SourceCurrency = new Currency("CZK"),
                        TargetCurrency = new Currency(row.CurrencyCode),
                        Value = row.Rate / row.Amount
                    });
                }
            }
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deserialize CNB XML.");
        }
        
        return Task.FromResult<IEnumerable<ExchangeRate>>(rates);
    }
}