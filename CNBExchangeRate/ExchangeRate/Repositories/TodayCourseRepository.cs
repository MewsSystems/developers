using ExchangeRate.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Repositories
{
  class TodayCourseRepository
  {

    private CoursesKeyName keyName;
    private LoadCourseFileRepository courseFile;

    public TodayCourseRepository()
    {
      keyName = new CoursesKeyName("Country", "Currency", "Amount", "Code", "Rate"); //instance class for Course key and name. It is second line with name
      courseFile = new LoadCourseFileRepository();//Class for read file from CNB url or Temp dir
    }

    /**
     * line split by txt in CNB
     */
    private string[] lineSplit(string line)
    {
      string[] items = line.Split('|');
      return items;
    }

    /**
     *set from the first line, values for the key(index) array be load
     */
    private CoursesKeyName SetKeyName(CoursesKeyName keyName, string line)
    {
      string[] items = lineSplit(line);
      int num = items.Count();
      int x;
      for (x = 0; x < num; x++)
      {
        keyName.setKey(x, items[x]);
      }
      return keyName;
    }

    /**
     *return Currency with all rating and information one line from CNB file
     */
    private List<Currency> SetCurrencyEntity(List<Currency> currencies, string line, CoursesKeyName keyName)
    {
      string[] items = lineSplit(line);
      int num = items.Count();
      int x;
      int CodeKey = keyName.CodeKey;//index array for code
      if(items[CodeKey] != null)
      {
        Currency currency = new Currency(items[CodeKey]);//create new entity Currency with code name
        
        for (x = 0; x < num; x++)
        {
          if (x == keyName.AmountKey)
          {
            currency.SetAmountByString(items[x]); //add Currency Amount 
          }
          else if (x == keyName.RateKey)
          {
            currency.SetRateByString(items[x]);//add Currency Rate  
          }
          else if (x == keyName.CountryKey)
          {
            currency.SetCountryByString(items[x]);//add Currency Country  
          }
        }
        
        currencies.Add(currency);//add Currency to list currencies
      }
      return currencies;
    } 
    

    /**
     *return Currency with all rating and information from CNB 
     */
    public IEnumerable<Currency> GetCurrency()
    {
      string line;
      int i = 0;
      List<Currency> currencies = new List<Currency>();
      StreamReader reader = courseFile.ReadCourse(); //load file from CNB URL or file in Temp dir

      //Read the stream to a string
      while ((line = reader.ReadLine()) != null)
      {
        if (i == 1)
        {
          keyName = SetKeyName(keyName, line);//add first line values for the key(index) array be load
        }
        else if (i > 1)
        {
          currencies=SetCurrencyEntity(currencies, line, keyName);//Currency with all rating and information one line from CNB file
        }
        i++;
      }
      return currencies;
    }
  }
}

