using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.Countries.CZE;

namespace ExchangeRateUpdater.Services.Countries.CZE;

public class CzeExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;

    public CzeExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        try
        {
            var response = await _httpClient.GetAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = XmlReader.Create(stream, new XmlReaderSettings
            {
                DtdProcessing = DtdProcessing.Parse
            });

            var serializer = new XmlSerializer(typeof(CnbExchangeRatesResponse));
            var result = (CnbExchangeRatesResponse)serializer.Deserialize(reader);


            return MapToExchangeRates(result, currencies);
        }
        catch (System.Exception e)
        {

            throw;
        }
    }

    private IEnumerable<ExchangeRate> MapToExchangeRates(
        CnbExchangeRatesResponse response,
        IEnumerable<Currency> requestedCurrencies)
    {
        const string baseCurrencyCode = "CZK";

        var requestedCodes = requestedCurrencies.Select(c => c.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);

        return response.Table.Rates
            .Where(rate => requestedCodes.Contains(rate.Code))
            .Select(rate =>
            {
                var source = new Currency(baseCurrencyCode);
                var target = new Currency(rate.Code);
                var value = rate.Rate / rate.Amount;

                return new ExchangeRate(source, target, value);
            });
    }
}
