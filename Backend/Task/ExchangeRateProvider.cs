using System;
using System.Collections.Generic;
using System.Xml;

namespace ExchangeRateUpdater
{
  public class ExchangeRateProvider
  {
    private static string ApiSourceUri = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
    private Dictionary<string, CurrencyRate> Cache { get; set; }
    private DateTime LastUpdate { get; set; } = DateTime.MinValue;

    private Dictionary<string, CurrencyRate> GetSourceData()
    {
      var now = DateTime.Now;
      if (Cache != null && LastUpdate.Date == now.Date) return Cache;
      var newCache = new Dictionary<string, CurrencyRate>();
      using (XmlTextReader reader = new XmlTextReader(ApiSourceUri))
      {
        while (reader.Read())
        {
          if (reader.NodeType == XmlNodeType.Element && reader.Name == "radek")
          {
            CurrencyRate currencyRate = parseCurrencyRateFromXml(reader);
            if (currencyRate.isValid)
              newCache.Add(currencyRate.Code, currencyRate);
          }
        }
      }
      Cache = newCache;
      LastUpdate = now;
      return Cache;
    }

    private CurrencyRate parseCurrencyRateFromXml(XmlTextReader reader)
    {
      var currencyRate = new CurrencyRate();
      while (reader.MoveToNextAttribute())
      {
        switch (reader.Name)
        {
          case "kod":
            currencyRate.Code = reader.Value;
            break;
          case "mnozstvi":
            int count;
            try
            {
              count = int.Parse(reader.Value);
            }
            catch
            {
              count = -1;
            }
            currencyRate.Count = count;
            break;
          case "kurz":
            decimal rate;
            try
            {
              rate = decimal.Parse(reader.Value);
            }
            catch
            {
              rate = -1;
            }
            currencyRate.Rate = rate;
            break;
          default:
            break;
        }
      }
      return currencyRate;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
    /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>

    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
      var sourceData = GetSourceData();
      var target = new Currency("CZK");

      foreach (var currency in currencies)
      {
        if (!sourceData.ContainsKey(currency.Code)) continue;
        yield return new ExchangeRate(
          currency,
          target,
          sourceData[currency.Code].Rate / sourceData[currency.Code].Count
          );
      }
    }
  }
}
