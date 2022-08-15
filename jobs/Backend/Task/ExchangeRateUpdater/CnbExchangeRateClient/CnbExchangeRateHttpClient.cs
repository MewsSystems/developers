using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater;

public class CnbExchangeRateHttpClient: ICnbExchangeRateHttpClient
{
    private const string ExchangeRatesUrlTemplate =
        "/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={0}";
    
    private readonly HttpClient _httpClient;
    private readonly IExchangeRateParser _parser;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CnbExchangeRateHttpClient(
        HttpClient httpClient, 
        IExchangeRateParser parser, 
        IDateTimeProvider dateTimeProvider)
    {
        _httpClient = httpClient;
        _parser = parser;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<IEnumerable<ExchangeRateLine>> GetTodaysExchangeRates(CancellationToken cancellationToken)
    {
        var currentDate = _dateTimeProvider.Current.ToString("dd.MM.yyyy");
        await using var stream = await _httpClient.GetStreamAsync(
            string.Format(ExchangeRatesUrlTemplate, currentDate), cancellationToken);

        return await _parser.ParseExchangeRateList(stream);
    }
}