using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace ExchangeRateUpdater.Support;

public class Parser : IParser
{
  private const string RowSelector = "/kurzy/tabulka/radek";
  private const string Currency = "kod";
  private const string Rate = "kurz";
  private const string Quantity = "mnozstvi";

  private static readonly NumberFormatInfo FormatInfo = new NumberFormatInfo() {
    NumberDecimalSeparator = ","
  };
  
  public IList<CZKRate> Parse(Stream stream)
  {
    var doc = new XmlDocument();
    doc.Load(stream);

    var nodes = doc.SelectNodes(RowSelector);

    var result = new List<CZKRate>();
    foreach (XmlNode node in nodes)
    {
      var rate = decimal.Parse(node.Attributes[Rate].InnerText, FormatInfo);
      var qty = int.Parse(node.Attributes[Quantity].InnerText, CultureInfo.InvariantCulture);
      if (qty != 1)
      {
        rate /= qty;
      }
      result.Add(new CZKRate(node.Attributes[Currency].InnerText, rate));
    }

    return result;
  }
}