using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExchangeRateUpdater.CzechNationalBank.HttpClient.Dtos;
using Microsoft.VisualBasic.FileIO;

namespace ExchangeRateUpdater.CzechNationalBank.HttpClient.Parsers
{
    public static class ExchangeRateResponseParser
    {
        static int NumberOfHeaderLines = 2;
        
        public static IEnumerable<ExchangeRateDto> Parse(string str)
        {
            var exchangeRates = new List<ExchangeRateDto>();
            
            try
            {
                using var parser = new TextFieldParser(CreateStreamFromString(str));

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("|");

                GetRidOfHeader(parser);

                string[] fields;
                while (!parser.EndOfData) 
                {
                    fields = parser.ReadFields();

                    if (fields is not null)
                    {
                        exchangeRates.Add(new ExchangeRateDto
                        {
                            Country = !string.IsNullOrEmpty(fields[0]) ? fields[0] : null,
                            CurrencyName = !string.IsNullOrEmpty(fields[1]) ? fields[1] : null,
                            Amount = int.TryParse(fields[2], out var amount) ? amount : null,
                            Currency = !string.IsNullOrEmpty(fields[3]) ? new Currency(fields[3]) : null,
                            Rate = decimal.TryParse(fields[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var rate) ? rate : null
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw new ParserException("Unable to parse exchange rate data", e);
            }

            return exchangeRates;
        }

        static void GetRidOfHeader(TextFieldParser parser)
        {
            for (int i = 0; i < NumberOfHeaderLines; i++)
            {
                if (!parser.EndOfData)
                {
                    parser.ReadLine();
                }
            }
        }

        static Stream CreateStreamFromString(string str)
        {
            var memoryStream = new MemoryStream();
            var streamWriter = new StreamWriter(memoryStream);
            streamWriter.Write(str);
            streamWriter.Flush();
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}