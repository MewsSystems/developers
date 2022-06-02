using System.Runtime.Serialization;
using System.Xml.Serialization;
using ExchangeRateUpdater.Core.Entities;
using ExchangeRateUpdater.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Infrastructure.Cnb;

public class ExchangeRatesClient : IExchangeRatesClient
{
    public ExchangeRatesClient(HttpClient httpClient, IOptions<CnbApiOptions> options)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(options);

        if (string.IsNullOrEmpty(options.Value.DailyRatesEndpoint))
        {
            throw new ArgumentException($"Missing '{nameof(CnbApiOptions.DailyRatesEndpoint)}' value.");
        }

        _httpClient = httpClient;
        _dailyRatesEndpoint = options.Value.DailyRatesEndpoint;
    }
    
    public async Task<ExchangeRates> GetTodayExchangeRatesAsync()
    {
        var httpResponseMessage = await _httpClient.GetAsync(_dailyRatesEndpoint);
        
        var serializer = new XmlSerializer(typeof(ExchangeRates));
        var byteArrayHttpResponseMessage = await httpResponseMessage.Content.ReadAsByteArrayAsync();
        await using Stream reader = new MemoryStream(byteArrayHttpResponseMessage);
        var deserializedObj = serializer.Deserialize(reader);

        if (deserializedObj is null)
        {
            throw new SerializationException("There was an error deserializing CNB XML object.");
        }

        return (ExchangeRates)deserializedObj;
    }
    
    private readonly HttpClient _httpClient;
    private readonly string _dailyRatesEndpoint;
}