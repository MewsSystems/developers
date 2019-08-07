using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater {
  public class CurrencyParser {
    public IDictionary<string, decimal> ParseResponse(string response) {
      IDictionary<string, decimal> currencies = new Dictionary<string, decimal>();
      if(string.IsNullOrEmpty(response))
        return currencies;
      var lines = response.Split('\n');
      for(int i = 0; i < lines.Length; i++) {
        try {
          var currency = ParseLine(lines[i]);
          if(currency == null) {
            continue;
          }
          if(!currencies.ContainsKey(currency.Item1)) {
            currencies.Add(currency.Item1, currency.Item2);
          }
        }
        catch(Exception e) {
          throw new Exception("Error parsing lines", e);
        }
      }
      return currencies;

    }

    private Tuple<string, decimal> ParseLine(string line) {
      if(string.IsNullOrEmpty(line))
        return null;
      var cells = line.Split('|');
      if(cells.Length < 4)
        return null;
      decimal value = 0m;
      decimal.TryParse(cells[4], out value);
      var currency = Tuple.Create(cells[3], value);
      return currency;
    }
  }
}
