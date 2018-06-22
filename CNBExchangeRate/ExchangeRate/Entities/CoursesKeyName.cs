using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Entities
{
  //Course key entity from second line in file
  class CoursesKeyName
  {
    public CoursesKeyName(string country, string currency, string amount, string code, string rate)
    {
      Country = country;
      Currency = currency;
      Amount = amount;
      Code = code;
      Rate = rate;
    }

    //Country Name
    public string Country { get; set; }

    //Currency Name
    public string Currency { get; set; }

    //Amount Name
    public string Amount { get; set; }

    //Code Currency Name
    public string Code { get; set; }

    //Rate Currency Name
    public string Rate { get; set; }

    //index in array by Country
    public int CountryKey { get; set; }

    //index in array by Currency
    public int CurrencyKey { get; set; }

    //index in array by Amount
    public int AmountKey { get; set; }

    //index in array by Code
    public int CodeKey { get; set; }

    //index in array by Rate
    public int RateKey { get; set; }

    //setter index in array by key name
    public void setKey(int key, string name)
    {
      if (name == Country)
      {
        CountryKey = key;
      }
      else if (name == Currency)
      {
        CurrencyKey = key;
      }
      else if (name == Amount)
      {
        AmountKey = key;
      }
      else if (name == Code)
      {
        CodeKey = key;
      }
      else if (name == Rate)
      {
        RateKey = key;
      }
    }
  }
}
