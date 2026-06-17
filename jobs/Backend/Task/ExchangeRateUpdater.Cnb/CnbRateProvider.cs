using ExchangeRateUpdater.Common;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb;

public class CnbRateProvider : IExchangeRateProvider
{
    private const string RelativeUri =
        "en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

    private const NumberStyles RateNumberStyle = NumberStyles.AllowDecimalPoint;

    private readonly HttpClient _httpClient;

    public CnbRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <inheritdoc/>
    /// <remarks>
    /// See https://www.cnb.cz/en/faq/Format-of-the-foreign-exchange-market-rates/ for format definition
    /// </remarks>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var selectedCurrencies = new HashSet<Currency>(currencies);
        return GetAllExchangeRatesAsync()
            .ToBlockingEnumerable(CancellationToken.None)
            .Where(rate => selectedCurrencies.Contains(rate.SourceCurrency));
    }

    public async IAsyncEnumerable<ExchangeRate> GetAllExchangeRatesAsync()
    {
        using var response = await _httpClient.GetAsync(RelativeUri);
        response.EnsureSuccessStatusCode();

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var reader = new StreamReader(contentStream);

        // skipping first 2 lines - date and header
        await reader.ReadLineAsync();
        if(reader.EndOfStream)
        {
            throw new FormatException("Endpoint returned invalid data.");
        }
        await reader.ReadLineAsync();

        while(!reader.EndOfStream)
        {
            yield return ParseLine(await reader.ReadLineAsync());
        }
    }

    public static ExchangeRate ParseLine(string input)
    {
        // assuming the defined format is respected
        // not opting for a full CSV parser as the data is not defined as CSV but strictly as values separated by |
        var split = input.Split('|'); // might be worthwile to try and benchmark a Span<T> based solution
        if(split.Length != 5) 
        {
            throwUnexpected();
        }

        // format is defined as Country|Currency|Amount|Code|Rate

        if(!Decimal.TryParse(split[2], RateNumberStyle, CultureInfo.InvariantCulture, out var amount)
            || !Decimal.TryParse(split[4], RateNumberStyle, CultureInfo.InvariantCulture, out var rawRate))
        {
            throwUnexpected();
        }
        
        return new ExchangeRate(
            new Currency(split[3]),
            new Currency("CZK"), // all rates are defined with CZK as target currency
            rawRate / amount // need to arrive to a 1:x rate instead of the listed amount-based rate
        );

        void throwUnexpected(){
            throw new ArgumentException($"Unexpected data format. Raw data: {input}", nameof(input));
        }
    }
}
