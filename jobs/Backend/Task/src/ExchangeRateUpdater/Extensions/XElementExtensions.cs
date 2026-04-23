using System.Xml.Linq;

namespace ExchangeRateUpdater.Extensions
{
    public static class XElementExtensions
    {
        public static string GetExchangeCode(this XElement rowValue) => rowValue.Attribute("kod")?.Value;
        public static int GetExchangeAmount(this XElement rowValue) => int.Parse(rowValue.Attribute("mnozstvi")?.Value ?? "1");

        public static decimal GetExchangeRate(this XElement rowValue) => decimal.Parse(rowValue.Attribute("kurz")?.Value.Replace(",", "."));
    }
}
