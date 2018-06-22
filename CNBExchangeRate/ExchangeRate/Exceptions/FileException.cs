using ExchangeRate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Exceptions
{
  /*
   * Exception for File or Connect to CNB
   */
  class FileException : Exception
  {
    private readonly Status _status;

    public Status Status
    {
      get { return _status; }
    }

    public FileException(Status status, string message, Exception innerException)
      : base(message, innerException)
    {
      this._status = status;
    }
  }
}
