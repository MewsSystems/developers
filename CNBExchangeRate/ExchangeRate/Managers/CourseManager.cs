using ExchangeRate.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExchangeRate.Entities;
using ExchangeRate.Exceptions;

namespace ExchangeRate.Managers
{
  class CourseManager
  {
    private TodayCourseRepository todayCourseRepository;
    private IEnumerable<Currency> currencies;

    public CourseManager()
    {
      todayCourseRepository = new TodayCourseRepository();
    }

    //list of all Code from CNB file with exchange
    public List<string> ListCode()
    {
      currencies = todayCourseRepository.GetCurrency();
      List<string> data = new List<string>();
      foreach (Currency currency in currencies)
      {
        data.Add(currency.Code);
      }
      return data;
    }

    //Currency entity by Code
    public Currency GetCurrency( string Code )
    {
      bool isFind = false;
      Currency currencyRet = new Currency(Code);
      currencies = todayCourseRepository.GetCurrency();// Currency with all rating anfd information from CNB 

      //find correct Currency Entity
      foreach (Currency currency in currencies)
      {
        if(currency.Code == Code)
        {
          currencyRet = currency;
          isFind = true;
          break;
        }
      }

      //Currency not found will cause Exception
      if (isFind==false)
      {
        throw new CourseException(Status.RATE_ERR, string.Format(@"Exchange not found {0}.", Code));
      }
      return currencyRet;
    }

    /*
     * get Total float by Code and Sum Currency
     */
    public float GetCurrencyTotal(string Code, int SumCurrency)
    {
      float total = 0;
      if( SumCurrency > 0 )
      {
        Currency currency = GetCurrency(Code);
        total = (SumCurrency / currency.Amount) * currency.Rate; 
      }
      return total;
    }

    /**
     * convert sum Currency to integer
     */
    public int convertSumToInt(string SumCurrency)
    {
      bool isNumeric = true;
      foreach (char c in SumCurrency)
      {
        if (!Char.IsNumber(c))
        {
          isNumeric = false;
          break;
        }
      }

      if( isNumeric == true )
      {
        return Convert.ToInt32(SumCurrency);
      }
      else
      {
        throw new SumException(Status.SUM_ERR, string.Format(@"Wrong format. It must be the only number."));
      }
    }
  }
}
