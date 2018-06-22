using ExchangeRate.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Exceptions
{
  /*
   * Exception for Course
   */
  class CourseException : Exception
  {
    private readonly Status _status;

    public Status Status
    {
      get { return _status; }
    }

    public CourseException(Status status, string message)
      : base(message)
    {
      this._status = status;
    }
  }
}
