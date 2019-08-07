using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ExchangeRateUpdater {
  public class ExchangeRateProvider {
    private const string API_ADDRESS = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date={0}";
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
    /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies) {
      var today = DateTime.Now.ToString("dd.MM.yyyy");
      var downloadedPrices = DownloadRates(today);
      var parser = new CurrencyParser();
      var parsedPrices = parser.ParseResponse(downloadedPrices);
      var exchangeRateList = new List<ExchangeRate>();

      Currency targetCurrency = new Currency("CZK");
      foreach(var sourceCurrency in currencies) {
        if(parsedPrices.ContainsKey(sourceCurrency.Code)) {
          var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, parsedPrices[sourceCurrency.Code]);
          exchangeRateList.Add(exchangeRate);
        }
      }

      return exchangeRateList;
    }

    private string DownloadRates(string today) {

      var address = string.Format(API_ADDRESS, today);
      var webRequest = WebRequest.Create(address);

      webRequest.Method = "GET";
      string respons = string.Empty;

      try {
        using(var response = webRequest.GetResponse()) {
          using(var reader = new StreamReader(response.GetResponseStream())) {
            return respons = reader.ReadToEnd();
          }
        }
      }
      catch(Exception e) {
        throw new Exception("Error connecting bank.", e);
      }
    }
  }
}
