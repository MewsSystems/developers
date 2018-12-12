using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace ExchangeRateUpdater
{
  public class ExchangeRateProvider
  {
    /// <summary>
    /// Get HTML code from a giver URL address.
    /// </summary>
    /// <param name="url">URL address of a web page.</param>
    /// <returns>Returns a string containing HTML code of a web page</returns>
    private string GetHTML(string url) 
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      if (response.StatusCode == HttpStatusCode.OK) 
      {
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = null;

        if (response.CharacterSet == null) 
        {
          readStream = new StreamReader(receiveStream);
        }
        else 
        {
          readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
        }

        string data = readStream.ReadToEnd();

        response.Close();
        readStream.Close();

        return data;
      }
      return null;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
    /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
      string url = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/";
      string html = GetHTML(url + "daily.jsp");

      // If there was some error while fetching HTML code.
      if (html == null) 
      {
        return Enumerable.Empty<ExchangeRate>();
      }

      // Finding a link to the text version of the table.
      Regex link_to_table_regex = new Regex("<a class=\"noprint\" href=\"(daily.txt.+?)\"");
      Match link_to_table_match = link_to_table_regex.Match(html);

      // Downloading HTML of the text version of the page.
      html = GetHTML(url + link_to_table_match.Groups[1]);

      // If there was some error while fetching HTML code.
      if (html == null) 
      {
        return Enumerable.Empty<ExchangeRate>();
      }

      using (StringReader reader = new StringReader(html)) 
      {
        // First two lines of the file are not needed.
        reader.ReadLine();
        reader.ReadLine();
        
        List<ExchangeRate> exchange_rates = new List<ExchangeRate>();

        // Going through all the other lines one by one.
        Regex rate_entry_regex = new Regex("(.+?)\\|(.+?)\\|(.+?)\\|(.+?)\\|(.+)");
        string line;
        while ((line = reader.ReadLine()) != null) 
        {
          Match rate_entry_match = rate_entry_regex.Match(line);
          string code_from = rate_entry_match.Groups[4].ToString();

          // If the code is in the given list of currencies, add it to the result.
          foreach (Currency currency in currencies)
          { 
            if (currency.Code == code_from) 
            {
              string code_to = "CZK";
              float  amount = float.Parse(rate_entry_match.Groups[3].ToString(), CultureInfo.InvariantCulture);
              float  rate = float.Parse(rate_entry_match.Groups[5].ToString(), CultureInfo.InvariantCulture) / amount;

              exchange_rates.Add(new ExchangeRate(new Currency(code_from), new Currency(code_to), (decimal)rate));
            }
          }
        }

        return exchange_rates;
      }
    }
  }
}
