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
    private List<ExchangeRate> exchange_rates;

    public ExchangeRateProvider() 
    {
      this.exchange_rates = new List<ExchangeRate>();
    }

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
    /// Goes through the text version of the exchange rates table and formats information to the format needed.
    /// </summary>
    /// <param name="table">"|"-separated text version of the exchange rate table.</param>
    /// <param name="currencies">List of currencies we would like to get info about.</param>
    private void ParseTable(string table, IEnumerable<Currency> currencies) 
    {
      using (StringReader reader = new StringReader(table)) 
      {
        // First two lines of the file are not needed.
        reader.ReadLine();
        reader.ReadLine();
        
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

              this.exchange_rates.Add(new ExchangeRate(new Currency(code_from), new Currency(code_to), (decimal)rate));
            }
          }
        }
      }
    }

    /// <summary>
    /// Returns a table with echange rates in an easy-to-parse TXT format.
    /// </summary>
    /// <param name="url">Link to the main page of the exchange rates table.</param>
    /// <param name="page">Prefix for the link (like "index.html").</param>
    /// <param name="link_to_table_regex">Regex for finding a link to the text version of the page.</param>
    /// <returns>String with "|"-separated exchange rates.</returns>
    private string GetTable(string url, string page, Regex link_to_table_regex) {
      // Downloading HTML code of the main exchange rate page.
      string html = GetHTML(url + page);

      // Finding a link to the text version of the table.
      Match link_to_table_match = link_to_table_regex.Match(html);

      // Returning the text version.
      return GetHTML(url + link_to_table_match.Groups[1]);
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
    /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
    {
      // Links to the main and the additional pages of exchange rates.
      string url_main = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/";
      string url_addit = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/other_currencies_fx_rates/";

      // Regular expressions for finding links to text versions of the tables.
      Regex link_to_table_regex_main = new Regex("<a class=\"noprint\" href=\"(daily.txt.+?)\"");
      Regex link_to_table_regex_addit = new Regex("<a class=\"noprint\" href=\"(fx_rates.txt.+?)\"");

      // Downloading text versions of the tables.
      string table_main = GetTable(url_main, "daily.jsp", link_to_table_regex_main);
      string table_addit = GetTable(url_addit, "fx_rates.jsp", link_to_table_regex_addit);

      // Parsing the tables to match a format we need.
      ParseTable(table_main, currencies);
      ParseTable(table_addit, currencies);

      // Returning the result calculated in the previous step.
      return this.exchange_rates;
    }
  }
}
