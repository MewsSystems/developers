using ExchangeRateUpdater.Model;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    public class ExchangeProviderParser
    {
        const char DELIMITER = '|';

        public static ExchangeRateProviderObject ParseContent(string content)
        {
            ExchangeRateProviderObject obj = null;

            if (!string.IsNullOrWhiteSpace(content))
            {
                // Get lines
                string[] arrLines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Make sure we have more than 2 lines (1st line is date, 2nd line is header)
                if (arrLines != null && arrLines.Length > 2)
                {
                    // First line (date)
                    string strDateLine = arrLines[0];

                    Match mtchDate = Regex.Match(strDateLine, @"(?<date>.*?)\s*#\s*(?<number>\d+)");
                    // Do we have date and number (id)?
                    if (mtchDate.Success)
                    {
                        obj = new ExchangeRateProviderObject();

                        // Parse date
                        if (mtchDate.Groups["date"].Success)
                        {
                            if (DateTime.TryParse(mtchDate.Groups["date"].Value, out DateTime dt))
                                obj.RateDate = dt;
                        }

                        // Parse Id
                        if (mtchDate.Groups["number"].Success)
                        {
                            if (int.TryParse(mtchDate.Groups["number"].Value, out int id))
                                obj.RateId = id;
                        }

                        // Parse data lines
                        obj.RateItems = ParseDelimitedValues(arrLines);
                    }
                }
            }

            return obj;
        }


        /// <summary>
        /// Parse data using reflection
        /// Changing the order of columns will not break the parser
        /// Reflection comes at cost, but the advantage of this method of dynamic parsing of data that will not break when colum order change
        /// </summary>
        /// <param name="arrLines"></param>
        /// <returns></returns>
        private static List<ExchangeRateProviderItem> ParseDelimitedValues(string[] arrLines)
        {
            List<ExchangeRateProviderItem> items = new List<ExchangeRateProviderItem>(arrLines.Length - 2);

            // Parse header
            string strHeader = arrLines[1];
            string[] colNames = strHeader.Split(DELIMITER);

            // We will skip the first 2 lines then read to end
            for (int i = 2; i < arrLines.Length; i++)
            {
                ExchangeRateProviderItem item = new ExchangeRateProviderItem();
                string[] values = arrLines[i].Split(DELIMITER);

                // Set properties using reflection
                for (int b = 0; b < colNames.Length; b++)
                {
                    Type type = item.GetType();
                    PropertyInfo propInfo = type.GetProperty(colNames[b],
                                                        BindingFlags.IgnoreCase
                                                        | BindingFlags.Public
                                                        | BindingFlags.Instance);
                    if (propInfo != null)
                        propInfo.SetValue(item, Convert.ChangeType(values[b], propInfo.PropertyType));
                }

                items.Add(item);
            }

            return items;
        }
    }
}
