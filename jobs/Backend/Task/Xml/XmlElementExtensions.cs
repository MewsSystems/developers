using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using ExchangeRateUpdater.Fluent;
using ExchangeRateUpdater.Structures;

namespace ExchangeRateUpdater.Xml
{
    public static class XmlElementExtensions
    {
        public static async Task<Fluent<Table>> CreateTable(this Task<Fluent<XmlElement>> fluentElementTask,
            IList<string> headerNames)
        {
            if (headerNames == default || headerNames.Count == 0)
            {
                throw new Exception("There must be at least one header name specified");
            }

            var fluentElement = await fluentElementTask;

            var tableBuilder = new TableBuilder();

            var headerIndex = 0;
            foreach (XmlNode thElement in fluentElement.Value.GetElementsByTagName("th"))
            {
                try
                {
                    var headerName = thElement.InnerText.Trim();
                    if (!headerNames.Contains(headerName))
                    {
                        continue;
                    }
                    tableBuilder.AddHeader((headerName, headerIndex));
                }
                finally
                {
                    headerIndex++;
                }
            }

            if (tableBuilder.HeaderNames.Count() != headerNames.Count)
            {
                throw new Exception($"Table do not contains all headers, expected was '{headerNames}', " +
                                    $"but found '{string.Join(", ", tableBuilder.HeaderNames)}'");
            }

            foreach (XmlNode tdElement in fluentElement.Value.GetElementsByTagName("td"))
            {
                tableBuilder.TryAddCellValue(() => tdElement.InnerText.Trim());
            }
           
            return fluentElement.Create(tableBuilder.Build());

        }
    }
}