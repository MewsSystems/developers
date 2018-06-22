using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Entities
{
  //Currency entity
  public class Currency
  {
    public Currency(string code)
    {
      Code = code;
    }

    /// <summary>
    /// Three-letter ISO 4217 code of the currency.
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Country Currency
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// setter Country Currency by string
    /// </summary>
    public void SetCountryByString(string CountryString)
    {
      Country = CountryString;
    }

    /// <summary>
    /// Amount Currency
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// setter Amount Currency by string
    /// </summary>
    public void SetAmountByString(string AmountString)
    {
      Amount = Convert.ToInt32(AmountString);
    }

    /// <summary>
    /// Rate Currency
    /// </summary>
    public float Rate { get; set; }

    /// <summary>
    /// setter Rate Currency by string
    /// </summary>
    public void SetRateByString(string RateString)
    {
      Rate = float.Parse(RateString, System.Globalization.CultureInfo.InvariantCulture);
    }
  }
}
