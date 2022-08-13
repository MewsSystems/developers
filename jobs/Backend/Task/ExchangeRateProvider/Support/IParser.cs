using System.Collections.Generic;
using System.IO;

namespace ExchangeRateUpdater.Support;

public interface IParser
{
  IList<CZKRate> Parse(Stream stream);
}