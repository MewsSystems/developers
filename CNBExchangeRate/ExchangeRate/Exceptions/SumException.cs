using ExchangeRate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Exceptions
{
  /*
   * Exception for Sum
   */
  class SumException : Exception
  {
    private readonly Status _status;

    public Status Status
    {
      get { return _status; }
    }

    public SumException(Status status, string message)
      : base(message)
    {
      this._status = status;
    }
  }
}
