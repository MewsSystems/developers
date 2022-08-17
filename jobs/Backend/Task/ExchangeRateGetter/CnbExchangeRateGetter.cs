using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ExchangeRateUpdater.Model;
using ExchangeRateUpdater.Utils;

namespace ExchangeRateUpdater.ExchangeRateGetter
{
    public class CnbExchangeRateGetter : IExchangeRateGetter
    {
        public async Task<IEnumerable<ICurrencyDetails>> GetTodaysExchangeRates()
        {
            var today = DateTime.Now;
            var request = CnbConstants.BASE_URL + String.Join(".", new[] { today.Day.ToString(), today.Month.ToString(), today.Year.ToString() });

            return await GetParsedResponse(request);
        }

        public async Task<IEnumerable<ICurrencyDetails>> GetParsedResponse(string requestUrl)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            int linesToSkip = CnbConstants.LINES_TO_SKIP;
            var currencyDetails = new List<CnbCurrencyDetails>();

            using (var response = await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    if (linesToSkip > 0)
                    {
                        linesToSkip--;
                    }
                    else
                    {
                        var validData = line.Split(CnbConstants.EXPECTED_FILE_SEPARATOR);

                        if (!ParserValidation(validData))
                            continue;

                        currencyDetails.Add(new CnbCurrencyDetails
                        {
                            Country = validData[0],
                            CurrecyExtended = validData[1],
                            Factor = Convert.ToDecimal(validData[2], new CultureInfo("en-US")),
                            Currency = new Currency(validData[3]),
                            Rate = Convert.ToDecimal(validData[4], new CultureInfo("en-US")),
                        });
                    }
                }
            }

            return currencyDetails;
        }

        public bool ParserValidation(string[] validData)
        {
            if (validData.Length != CnbConstants.EXPECTED_ROW_SIZE)
            {
                Console.WriteLine($"An entry in the exchange rate file does not contain expected format - skipping line"); //this could be writen to a logger
                return false;
            }
            if (!Decimal.TryParse(validData[2], NumberStyles.Number, new CultureInfo("en-US"), out var amount))
            {
                Console.WriteLine($"Amount is not a valid number"); //this could be writen to a logger
                return false;
            }

            if (!Decimal.TryParse(validData[4], NumberStyles.Number, new CultureInfo("en-US"), out var exchangeRate))
            {
                Console.WriteLine($"Exchange Rate is not a valid number"); //this could be writen to a logger
                return false;
            }

            return true;
        }
    }
}
