﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Dtos;
using Microsoft.VisualBasic.FileIO;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Parsers
{
    public static class ExchangeRateResponseParser
    {
        static int NumberOfHeaderLines = 2;
        
        public static IEnumerable<ExchangeRateDto> Parse(Stream stream)
        {
            var exchangeRates = new List<ExchangeRateDto>();
            
            try
            {
                using var parser = new TextFieldParser(stream);

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
    }
}