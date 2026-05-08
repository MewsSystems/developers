using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater.Ext;

public class CnbClient : ICurrencyRateClient
{
    private const string TxtUrl = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt";
    private const string DateFmt = "dd.MM.YYYY";
    private static readonly Currency TargetRate = new("CZK");

    public async Task<List<ExchangeRate>> GetExchangeRates(DateOnly forDate)
    {
        using var client = new HttpClient();
        var fullUrl = TxtUrl + "?date=" + forDate.ToString(DateFmt);
        var response = await client.GetStringAsync(fullUrl);
        var parsedResponse = CnbTxtParser.ParseResponse(response);

        return parsedResponse.Select(ToExchangeRate).ToList();
    }

    private static ExchangeRate ToExchangeRate(CnbFxRow row)
    {
        // Some exchange rates are reported in 100s, e.g. JPY, so adjust rate accordingly.
        var adjustedRate = row.Rate / row.Amount;
        return new ExchangeRate(row.Currency, TargetRate, adjustedRate);
    }
}
