using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Entities
{
  //class for error status
  public enum Status : int
  {
    WEB_CLIENT_ERR = 1,
    SAVE_FILE_ERR = 2,
    LOAD_FILE_ERR = 3,
    RATE_ERR = 4,
    SUM_ERR = 5,
  };
}
